namespace AlumnosCSVImporter.Models
{
    public class Alumno {
        public int Id { get; set; }
        public required string Apellido { get; set;}
        public required string Nombre { get; set;}
        public required string NroDocumento { get; set;}
        public required TipoDocumento TipoDocumento { get; set;}
        public required string FechaNacimiento { get; set;}
        public required string Sexo { get; set;}
        public required int NroLegajo { get; set;}
        public required DateTime FechaIngreso { get; set;}
    }
}