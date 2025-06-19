using AlumnosCSVImporter.Data;

namespace AlumnosCSVImporter.Tests.Data;

public class DatabaseTests : IDisposable
{
    private readonly DatabaseContext _db;

    public DatabaseTests(DatabaseContextFactory? factory = null)
    {
        factory ??= new DatabaseContextFactory(); 
        _db = factory.CreateContext();
    }

    public void Dispose()
    {
        _db.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void IsConnectedToDatabase()
    {
        Assert.Equal(System.Data.ConnectionState.Open, _db.Connection.State);
    }
}
