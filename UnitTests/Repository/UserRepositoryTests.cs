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
    
}

public class User
{
}

public interface IUnitOfWork
{
    IRepository<T> Repository<T>() where T : class;
}

public interface IRepository<T> where T: class
{
    IEnumerable<T> GetAll();
}

public class Repository<T> : IRepository<T> where T : class
{
    private DBContext _context;

    public Repository(DBContext context)
    {
        _context = context;
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Find<T>();
    }
}

public class DBContext
{
    public IEnumerable<T> Find<T>()
    {
        return [];
    }
}

