using apidemo.Context;
using apidemo.Entities;

namespace apidemo.Repository;

public class UserRepository : GenericRepository<Users>,IUserRepository
{
    private IUserRepository _userRepositoryImplementation;

    public UserRepository(MySQLDBContext context) : base(context)
    {
    }

    public Users login(string username, string password)
    {
        var user = _context.Users
            .Where(u => u.Username.Equals(username))
            .Where(u => u.Password.Equals(password)).FirstOrDefault();
        return user;
    }
}