using System.Globalization;
using AlumnosCSVImporter.Mapping;
using AlumnosCSVImporter.Models;
using AlumnosCSVImporter.Services;
using CsvHelper;
using CsvHelper.Configuration;

namespace AlumnosCSVImporter.Import;

public class AlumnoImporter(AlumnoService Service)
{
    public void Import(string path)
    {
        using var reader = new StreamReader(path);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture);
        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<AlumnoMap>();
        int count = 0;
        List<Alumno> alumnos = [];
        foreach (var alumno in csv.GetRecords<Alumno>())
        {
            count++;
            alumnos.Add(alumno);
            if (count >= 20000)
            {
                Service.CreateMultiple(alumnos);
                alumnos.Clear();
                count = 0;
            }
        }
        if (alumnos.Count > 0)
        {
            Service.CreateMultiple(alumnos);
        }
    }
}