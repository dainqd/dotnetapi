using apidemo.Entities;

namespace apidemo.Repository;

public interface IUserRepository : IGenericRepository<Users>
{
    Users login(string username, string password);
}