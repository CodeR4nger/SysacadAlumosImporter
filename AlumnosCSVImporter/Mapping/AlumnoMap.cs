using AlumnosCSVImporter.Models;
using CsvHelper.Configuration;

namespace AlumnosCSVImporter.Mapping;
public class AlumnoMap : ClassMap<Alumno>
{
    public AlumnoMap()
    {
        Map(p => p.Apellido).Name("apellido");
        Map(p => p.Nombre).Name("nombre");
        Map(p => p.NroDocumento).Name("nro_documento");
        Map(p => p.TipoDocumento).Name("tipo_documento");
        Map(p => p.FechaNacimiento).Name("fecha_nacimiento");
        Map(p => p.Sexo).Name("sexo");
        Map(p => p.NroLegajo).Name("nro_legajo");     
        Map(p => p.FechaIngreso).Name("fecha_ingreso").TypeConverterOption.Format("yyyy-MM-dd");                   
    }
}