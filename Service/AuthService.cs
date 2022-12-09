using apidemo.Context;
using apidemo.Entities;
using apidemo.Models.Auth;
using apidemo.Repository;

namespace apidemo.Service;

public class AuthService : IAuthService
{
    private UserRepository _userRepository;

    public AuthService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Users login(AuthenticateRequest authenticateRequest)
    {
        
        throw new NotImplementedException();
    }

    public Users register(Users users)
    {
        throw new NotImplementedException();
    }
}