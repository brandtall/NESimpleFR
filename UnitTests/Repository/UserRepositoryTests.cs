using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitTests.Repository;

public class UserRepositoryTests
{
    [Fact]
    public async Task ShouldReturnAllUsers()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var listOfUsers = new List<User>(){new User()};
        unitOfWork.Setup(u => u.Repository<User>().GetAll()).ReturnsAsync(listOfUsers);

        var users = await unitOfWork.Object.Repository<User>().GetAll();

        Assert.NotEmpty(users);
    }
    
    [Fact]
    public async Task ShouldReturnUserById()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        const int id = 6;
        unitOfWork.Setup(u => u.Repository<User>().FindById(id)).ReturnsAsync(new User()
        {
            Id = id
        });

        var user = await unitOfWork.Object.Repository<User>().FindById(id);

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
    Task<List<T>> GetAll();
    Task<T?> FindById(int i);
}

public class Repository<T> : IRepository<T> where T : class
{
    private readonly UserDbContext _context;

    public Repository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<List<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> FindById(int i)
    {
        return await _context.Set<T>().FindAsync(i);
    }
}

public class UserDbContext : DbContext
{
    protected UserDbContext(DbContextOptions options) : base(options)
    {
    }

    private DbSet<User> Users { get; set;}
}

