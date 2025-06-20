using CsvHelper;
using CsvHelper.Configuration;
using AlumnosCSVImporter.Models;
using AlumnosCSVImporter.Mapping;
using AlumnosCSVImporter.Services;
using System.Globalization;

namespace AlumnosCSVImporter.Import;

public class AlumnoImporter(List<AlumnoService> services, int batchSize = 20000)
{
    private readonly List<AlumnoService> _services = services;
    private readonly int _batchSize = batchSize;

    public void Import(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"El archivo '{path}' no existe.");

        var ongoingTasks = new Dictionary<AlumnoService, Task>();
        var serviceQueue = new Queue<AlumnoService>(_services);
        var taskLock = new object();

        using var reader = new StreamReader(path);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture);
        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<AlumnoMap>();

        var currentBatch = new List<Alumno>(_batchSize);

        foreach (var alumno in csv.GetRecords<Alumno>())
        {
            currentBatch.Add(alumno);

            if (currentBatch.Count >= _batchSize)
            {
                var batchToInsert = new List<Alumno>(currentBatch);
                currentBatch.Clear();

                AlumnoService serviceToUse;

                lock (taskLock)
                {
                    while (serviceQueue.Count == 0)
                    {
                        var completedTask = Task.WhenAny(ongoingTasks.Values).Result;

                        var freedService = ongoingTasks.First(kvp => kvp.Value == completedTask).Key;
                        ongoingTasks.Remove(freedService);
                        serviceQueue.Enqueue(freedService);
                    }

                    serviceToUse = serviceQueue.Dequeue();

                    var task = Task.Run(() =>
                    {
                        serviceToUse.CreateMultiple(batchToInsert);
                    });

                    ongoingTasks[serviceToUse] = task;
                }
            }
        }

        if (currentBatch.Count > 0)
        {
            AlumnoService serviceToUse;

            lock (taskLock)
            {
                while (serviceQueue.Count == 0)
                {
                    var completedTask = Task.WhenAny(ongoingTasks.Values).Result;
                    var freedService = ongoingTasks.First(kvp => kvp.Value == completedTask).Key;
                    ongoingTasks.Remove(freedService);
                    serviceQueue.Enqueue(freedService);
                }

                serviceToUse = serviceQueue.Dequeue();

                var task = Task.Run(() =>
                {
                    serviceToUse.CreateMultiple(currentBatch);
                });

                ongoingTasks[serviceToUse] = task;
            }
        }

        Task.WaitAll(ongoingTasks.Values.ToArray());
    }
}
