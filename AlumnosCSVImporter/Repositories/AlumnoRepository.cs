using AlumnosCSVImporter.Data;
using AlumnosCSVImporter.Models;
using System.Globalization;
namespace AlumnosCSVImporter.Repositories;

public class AlumnoRepository(DatabaseContext context)
{
    protected readonly DatabaseContext _context = context;
    
    private static Alumno MapToAlumno(Dictionary<string, object> row)
    {
        return new Alumno
        {
            Id = Convert.ToInt32(row["Id"]),
            Apellido = row["Apellido"].ToString() ?? string.Empty,
            Nombre = row["Nombre"].ToString() ?? string.Empty,
            NroDocumento = row["NroDocumento"].ToString() ?? string.Empty,
            TipoDocumento = (TipoDocumento)Convert.ToInt32(row["TipoDocumento"]),
            FechaNacimiento = row["FechaNacimiento"].ToString() ?? string.Empty,
            Sexo = row["Sexo"].ToString() ?? string.Empty,
            NroLegajo = Convert.ToInt32(row["NroLegajo"]),
            FechaIngreso = DateTime.Parse(row["FechaIngreso"].ToString() ?? string.Empty)
        };
    }

    public Alumno Create(Alumno entity)
    {
        var sql = $@"
            INSERT INTO ""Alumnos"" (""Apellido"", ""Nombre"", ""NroDocumento"", ""TipoDocumento"", ""FechaNacimiento"", ""Sexo"", ""NroLegajo"", ""FechaIngreso"")
            VALUES (
                '{entity.Apellido.Replace("'", "''")}',
                '{entity.Nombre.Replace("'", "''")}',
                '{entity.NroDocumento.Replace("'", "''")}',
                {(int)entity.TipoDocumento},
                '{entity.FechaNacimiento}', -- Asumo string tipo 'yyyy-MM-dd' o similar
                '{entity.Sexo}',
                {entity.NroLegajo},
                '{entity.FechaIngreso.ToString("yyyy-MM-dd")}'
            )
            RETURNING ""Id"";
        ";

        var result = _context.ExecuteQuery(sql);

        if (result.Count > 0 && result[0].ContainsKey("Id"))
        {
            entity.Id = Convert.ToInt32(result[0]["Id"]);
            return entity;
        }

        throw new Exception("No se pudo crear el alumno.");
    }
    public List<Alumno> CreateMultiple(List<Alumno> entities)
    {
        if (entities == null || entities.Count == 0)
            return new List<Alumno>();

        var valuesList = new List<string>();

        for (int i = 0; i < entities.Count; i++)
        {
            var e = entities[i];
            var values = $@"(
                '{e.Apellido.Replace("'", "''")}',
                '{e.Nombre.Replace("'", "''")}',
                '{e.NroDocumento.Replace("'", "''")}',
                {(int)e.TipoDocumento},
                '{e.FechaNacimiento}',
                '{e.Sexo}',
                {e.NroLegajo},
                '{e.FechaIngreso.ToString("yyyy-MM-dd")}'
            )";
            valuesList.Add(values);
        }

        var sql = $@"
            INSERT INTO ""Alumnos"" (""Apellido"", ""Nombre"", ""NroDocumento"", ""TipoDocumento"", ""FechaNacimiento"", ""Sexo"", ""NroLegajo"", ""FechaIngreso"")
            VALUES
            {string.Join(",\n", valuesList)}
            RETURNING ""Id"";
        ";

        var result = _context.ExecuteQuery(sql);

        if (result.Count != entities.Count)
            throw new Exception("No se pudo crear todos los alumnos.");

        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].Id = Convert.ToInt32(result[i]["Id"]);
        }

        return entities;
    }

    public Alumno? SearchById(int id)
    {
        var sql = $"SELECT * FROM \"Alumnos\" WHERE \"Id\" = {id};";
        var results = _context.ExecuteQuery(sql);

        if (results.Count == 0)
            return null;

        return MapToAlumno(results[0]);
    }

    public List<Alumno> SearchAll()
    {
        var sql = "SELECT * FROM \"Alumnos\";";
        var results = _context.ExecuteQuery(sql);

        return [.. results.Select(MapToAlumno)];
    }

    public Alumno Update(Alumno entity)
    {
        var sql = $@"
                UPDATE ""Alumnos"" SET
                    ""Apellido"" = '{entity.Apellido.Replace("'", "''")}',
                    ""Nombre"" = '{entity.Nombre.Replace("'", "''")}',
                    ""NroDocumento"" = '{entity.NroDocumento.Replace("'", "''")}',
                    ""TipoDocumento"" = {(int)entity.TipoDocumento},
                    ""FechaNacimiento"" = '{entity.FechaNacimiento}',
                    ""Sexo"" = '{entity.Sexo}',
                    ""NroLegajo"" = {entity.NroLegajo},
                    ""FechaIngreso"" = '{entity.FechaIngreso.ToString("yyyy-MM-dd")}'
                WHERE ""Id"" = {entity.Id};
            ";

        var rowsAffected = _context.ExecuteNonQuery(sql);

        if (rowsAffected == 0)
            throw new Exception($"No se encontró el alumno con id {entity.Id} para actualizar.");

        return entity;
    }

    public bool DeleteById(int id)
    {
        var sql = $"DELETE FROM \"Alumnos\" WHERE \"Id\" = {id};";
        var rowsAffected = _context.ExecuteNonQuery(sql);

        return rowsAffected > 0;
    }
}
