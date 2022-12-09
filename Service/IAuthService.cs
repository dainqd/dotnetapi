using apidemo.Entities;
using apidemo.Models.Auth;
using apidemo.Repository;

namespace apidemo.Service;

public interface IAuthService
{
    Users login(AuthenticateRequest authenticateRequest);
    Users register(Users users);
}