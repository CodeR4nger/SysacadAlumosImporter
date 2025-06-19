using AlumnosCSVImporter.Models;
using AlumnosCSVImporter.Tests.Utils;
using AlumnosCSVImporter.Services;
using AlumnosCSVImporter.Repositories;
namespace AlumnosCSVImporter.Tests.Models;


public class AlumnoTests : BaseTestDB
{
    protected readonly AlumnoService Service;

    public AlumnoTests()
    {
        Service = new AlumnoService(new AlumnoRepository(Context));
    }

    private static Alumno CreateEntity()=> new()
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
    protected static void CheckEntity(Alumno alumno)
    {
        Assert.NotNull(alumno);
        Assert.Equal("Gomez", alumno.Apellido);
        Assert.Equal("Juan", alumno.Nombre);
        Assert.Equal("12345678", alumno.NroDocumento);
        Assert.Equal(TipoDocumento.DNI, alumno.TipoDocumento);
        Assert.Equal("2000-01-01", alumno.FechaNacimiento);
        Assert.Equal("M", alumno.Sexo);
        Assert.Equal(1001, alumno.NroLegajo);
        Assert.Equal(new DateTime(2020, 3, 1), alumno.FechaIngreso);
    }
    protected Alumno Create(Alumno entity) => Service.Create(entity);

    protected List<Alumno> CreateMultiple(List<Alumno> entities) => Service.CreateMultiple(entities);

    protected Alumno? GetById(int id) => Service.SearchById(id);

    protected IList<Alumno> GetAll() => Service.SearchAll();

    protected Alumno Update(Alumno entity) => Service.Update(entity);

    protected bool Delete(int id) => Service.DeleteById(id);

    protected static int GetId(Alumno entity) => entity.Id;

    protected static void ModifyEntity(Alumno entity)
    {
        entity.Apellido = "Sanchez";
    }
    protected static void CheckUpdatedEntity(Alumno alumno)
    {
        Assert.Equal("Sanchez", alumno.Apellido);
    }

    [Fact]
    public void CanCreateEntity()
    {
        var entity = CreateEntity();
        var created = Create(entity);
        CheckEntity(created);
        Assert.True(GetId(created) > 0);
    }
    
    [Fact]
    public void CanReadEntity()
    {
        var entity = Create(CreateEntity());
        var fetched = GetById(GetId(entity));
        Assert.NotNull(fetched);
        CheckEntity(fetched);
    }

    [Fact]
    public void CanReadAllEntities()
    {
        Create(CreateEntity());
        Create(CreateEntity());
        var all = GetAll();
        Assert.True(all.Count >= 2);
    }
    [Fact]
    public void CanCreateMultipleEntity()
    {
        List<Alumno> entities = [CreateEntity(), CreateEntity()];
        CreateMultiple(entities);
        var all = GetAll();
        Assert.True(all.Count >= 2);
    }
    
    [Fact]
    public void CanUpdateEntity()
    {
        var entity = Create(CreateEntity());
        ModifyEntity(entity);
        var updated = Update(entity);
        Assert.Equal(GetId(entity), GetId(updated));
        var fetched = GetById(GetId(entity));
        Assert.NotNull(fetched);
        CheckUpdatedEntity(fetched);
    }

    [Fact]
    public void CanDeleteEntity()
    {
        var entity = Create(CreateEntity());
        var deleted = Delete(GetId(entity));
        Assert.True(deleted);
        var found = GetById(GetId(entity));
        Assert.Null(found);
    }
}