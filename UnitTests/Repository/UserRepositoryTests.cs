using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitTests.Repository;

public class UserRepositoryTests
{
    [Fact]
    public void ShouldReturnAllUsers()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Repository<User>().GetAll()).Returns(new List<User>(){new User()});

        var users = unitOfWork.Object.Repository<User>().GetAll();

        Assert.NotEmpty(users);
    }
    
    [Fact]
    public void ShouldReturnUserById()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        const int id = 6;
        unitOfWork.Setup(u => u.Repository<User>().FindById(id)).Returns(new User()
        {
            Id = id
        });

        var user = unitOfWork.Object.Repository<User>().FindById(id);

        Assert.NotNull(user);
        Assert.Equal(id, user.Id);
    }
    
}

public class User
{
    public int Id { get; set; }
}

public interface IUnitOfWork
{
    IRepository<T> Repository<T>() where T : class;
}

public interface IRepository<T> where T: class
{
    IEnumerable<T> GetAll();
    T FindById(int i);
}

public class Repository<T> : IRepository<T> where T : class
{
    private DbContext _context;

    public Repository(DbContext context)
    {
        _context = context;
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Find<T>();
    }

    public T FindById(int i)
    {
        return _context.FindById<T>(i);
    }
}

public interface IDbContext
{
    IEnumerable<T> Find<T>();
}

public class DbContext : IDbContext
{
    public IEnumerable<T> Find<T>()
    {
        return [];
    }

    public T FindById<T>(int i)
    {
        return default;
    }
}

