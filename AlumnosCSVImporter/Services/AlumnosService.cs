using AlumnosCSVImporter.Repositories;
using AlumnosCSVImporter.Models;
using AlumnosCSVImporter.Data;
namespace AlumnosCSVImporter.Services;

public class AlumnoService(AlumnoRepository repository)
{
    protected readonly AlumnoRepository _repository = repository;

    public Alumno Create(Alumno entity) => _repository.Create(entity);

    public List<Alumno> CreateMultiple(List<Alumno> entities) => _repository.CreateMultiple(entities);
    public Alumno? SearchById(int id) => _repository.SearchById(id);
    public List<Alumno> SearchAll() => _repository.SearchAll();
    public Alumno Update(Alumno entity) => _repository.Update(entity);
    public bool DeleteById(int id) => _repository.DeleteById(id);
    
}


public class AlumnoServiceFactory(IDatabaseContextFactory contextFactory)
{
    private readonly IDatabaseContextFactory _contextFactory = contextFactory;

    public AlumnoService CreateService()
    {
        var context = _contextFactory.CreateContext();
        var repository = new AlumnoRepository(context);
        return new AlumnoService(repository);
    }
    public List<AlumnoService> GetServiceList(int count)
    {
        var services = new List<AlumnoService>(capacity: count);

        for (int i = 0; i < count; i++)
        {
            services.Add(CreateService());
        }

        return services;
    }
}