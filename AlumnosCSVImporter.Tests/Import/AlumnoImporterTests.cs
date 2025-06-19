using AlumnosCSVImporter.Mapping;
using AlumnosCSVImporter.Models;
using AlumnosCSVImporter.Services;
using AlumnosCSVImporter.Tests.Utils;
using AlumnosCSVImporter.Import;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
namespace AlumnosCSVImporter.Tests.Import;

public class AlumnoImporterTests : BaseTestDB
{
    private readonly string TestFilePath = "alumnosTest.csv";
    private readonly AlumnoService Service;
    public AlumnoImporterTests()
    {
        Service = new AlumnoService(new Repositories.AlumnoRepository(Context));
    }
    private Alumno CreateCSV()
    {
        var alumno = TestDataFactory.CreateAlumno();
        List<Alumno> alumnos = [alumno];
        var config = new CsvConfiguration(CultureInfo.InvariantCulture);
        using var writer = new StreamWriter(TestFilePath);
        using var csv = new CsvWriter(writer, config);
        csv.Context.RegisterClassMap<AlumnoMap>();
        csv.WriteRecords(alumnos);
        return alumno;
    }
    public override void Dispose()
    {
        if (File.Exists(TestFilePath))
            File.Delete(TestFilePath);
        base.Dispose();
        GC.SuppressFinalize(this);
    }
    [Fact]
    public void CanImportCSV()
    {
        Alumno alumno = CreateCSV();
        var importer = new AlumnoImporter(Service);
        importer.Import(TestFilePath);
        var alumnos = Service.SearchAll();
        Assert.Single(alumnos);
        var fetchedAlumno = alumnos[0];
        alumno.Id = fetchedAlumno.Id;
        Assert.NotNull(fetchedAlumno);
        Assert.Equivalent(alumno, fetchedAlumno,false);
    }
}