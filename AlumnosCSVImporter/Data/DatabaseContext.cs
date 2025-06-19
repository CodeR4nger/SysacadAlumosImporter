using Npgsql;
using AlumnosCSVImporter.Utils;

namespace AlumnosCSVImporter.Data;

public class DatabaseContext : IDisposable
{
    private readonly NpgsqlConnection _connection;

    public DatabaseContext(string connectionString)
    {
        _connection = new NpgsqlConnection(connectionString);
        _connection.Open();
    }

    public NpgsqlConnection Connection => _connection;

    /// <summary>
    /// Ejecuta una consulta SQL (SELECT) y devuelve los resultados en forma de lista de diccionarios.
    /// </summary>
    public List<Dictionary<string, object>> ExecuteQuery(string sql)
    {
        using var command = new NpgsqlCommand(sql, _connection);
        using var reader = command.ExecuteReader();

        var results = new List<Dictionary<string, object>>();

        while (reader.Read())
        {
            var row = new Dictionary<string, object>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                row[reader.GetName(i)] = reader.GetValue(i);
            }

            results.Add(row);
        }

        return results;
    }

    /// <summary>
    /// Ejecuta una consulta SQL de tipo INSERT/UPDATE/DELETE.
    /// </summary>
    public int ExecuteNonQuery(string sql)
    {
        using var command = new NpgsqlCommand(sql, _connection);
        return command.ExecuteNonQuery();
    }
    
    public void Dispose()
    {
        if (_connection.State != System.Data.ConnectionState.Closed)
        {
            _connection.Close();
            _connection.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}
public interface IDatabaseContextFactory
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="DatabaseContext"/>.
    /// </summary>
    /// <returns>Una instancia de <see cref="DatabaseContext"/> con la conexión abierta.</returns>
    DatabaseContext CreateContext();
}

public class DatabaseContextFactory : IDatabaseContextFactory
{
    private readonly IEnvironmentHandler _envHandler;

    public DatabaseContextFactory() : this(new EnvironmentHandler())
    {
    }

    public DatabaseContextFactory(IEnvironmentHandler envHandler)
    {
        _envHandler = envHandler;
        _envHandler.Load();
    }

    public DatabaseContext CreateContext()
    {
        string connectionString = $"Host={_envHandler.Get("POSTGRES_HOST")};" +
                                  $"Username={_envHandler.Get("POSTGRES_USER")};" +
                                  $"Password={_envHandler.Get("POSTGRES_PASSWORD")};" +
                                  $"Database={_envHandler.Get("POSTGRES_DB")};";

        return new DatabaseContext(connectionString);
    }
}

