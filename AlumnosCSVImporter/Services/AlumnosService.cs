using AlumnosCSVImporter.Repositories;
using AlumnosCSVImporter.Models;
namespace AlumnosCSVImporter.Services;

public class AlumnoService
{
    protected readonly AlumnoRepository _repository;

    public AlumnoService(AlumnoRepository repository)
    {
        _repository = repository;
    }

    public Alumno Create(Alumno entity) => _repository.Create(entity);

    public List<Alumno> CreateMultiple(List<Alumno> entities) => _repository.CreateMultiple(entities);
    public Alumno? SearchById(int id) => _repository.SearchById(id);
    public List<Alumno> SearchAll() => _repository.SearchAll();
    public Alumno Update(Alumno entity) => _repository.Update(entity);
    public bool DeleteById(int id) => _repository.DeleteById(id);
    
}
