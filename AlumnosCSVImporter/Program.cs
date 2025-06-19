// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using AlumnosCSVImporter.Data;
using AlumnosCSVImporter.Import;
using AlumnosCSVImporter.Repositories;
using AlumnosCSVImporter.Services;

Console.WriteLine("IMPORTADOR CSV");
const string FILE_PATH = "alumnos.csv";

Console.WriteLine("Conectando a base de datos");
var databaseContext = new DatabaseContextFactory().CreateContext();
Console.WriteLine("Conexion establecida exitosamente");
Console.WriteLine("Verificando tabla Alumnos");
databaseContext.EnsureAlumnoTable();
Console.WriteLine("Creando importador");
var stopwatch = Stopwatch.StartNew();
var importer = new AlumnoImporter(new AlumnoService(new AlumnoRepository(databaseContext)));
Console.WriteLine("Iniciando importacion");
importer.Import(FILE_PATH);
Console.WriteLine("Importacion finalizada exitosamente");
stopwatch.Stop();
Console.WriteLine($"Tiempo transcurrido: {stopwatch.ElapsedMilliseconds} ms");