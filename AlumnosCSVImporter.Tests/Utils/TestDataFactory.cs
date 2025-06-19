namespace AlumnosCSVImporter.Tests.Utils;

using System.Data;
using AlumnosCSVImporter.Models;

public static class TestDataFactory
{
    public static Alumno CreateAlumno() => new()
    {
        Apellido = "Gomez",
        Nombre = "Juan",
        NroDocumento = "12345678",
        TipoDocumento = TipoDocumento.DNI,
        FechaNacimiento = "2000-01-01",
        Sexo = "M",
        NroLegajo = 1001,
        FechaIngreso = new DateTime(2020, 3, 1)
    };
}
