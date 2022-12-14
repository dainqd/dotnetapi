using apidemo.Authorization;
using apidemo.Context;
using apidemo.Entities;
using apidemo.Models.Users;

namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net;

public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    IEnumerable<Users> GetAll();
    Users GetById(int id);
    void Register(RegisterRequest model);
    void ChangPass(int id, ChangePasswordRequest model);
    void UpdateInfo(int id, UpdateRequest model);
    void Delete(int id);
    public void Create(CreateRequest model);
}

public class UserService : IUserService
{
    private MySQLDBContext _context;
    private IJwtUtils _jwtUtils;
    private readonly IMapper _mapper;

    public UserService(
        MySQLDBContext context,
        IJwtUtils jwtUtils,
        IMapper mapper)
    {
        _context = context;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _context.Users.SingleOrDefault(x => x.Username == model.Username);

        // validate
        if (user == null || !BCrypt.Verify(model.Password, user.Password))
            throw new AppException("Username or password is incorrect");

        // authentication successful
        var response = _mapper.Map<AuthenticateResponse>(user);
        response.Token = _jwtUtils.GenerateToken(user);
        return response;
    }

    public IEnumerable<Users> GetAll()
    {
        return _context.Users;
    }

    public Users GetById(int id)
    {
        return getUser(id);
    }

    public void Register(RegisterRequest model)
    {
        // validate
        if (_context.Users.Any(x => x.Username == model.Username))
            throw new AppException("Username '" + model.Username + "' is already taken");
        
        if (model.Username == null || model.Password == null)
            throw new AppException("Username or Password invalid!");

        if(model.Password.Length<6)
            throw new AppException("Password invalid!");
        
        if (model.Password != model.ConfirmPassword)
            throw new AppException("Password or Password Confirm incorrect!");

        // map model to new user object
        var user = _mapper.Map<Users>(model);

        // hash password
        user.Password = BCrypt.HashPassword(model.Password);

        // save user
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void ChangPass(int id, ChangePasswordRequest model)
    {
        var user = getUser(id);
        
        if(model.Password.Length<6 || model.OldPassword.Length<6)
            throw new AppException("Password invalid!");

        if (model.Password != model.ConfirmPassword)
            throw new AppException("Password or Password Confirm incorrect!");

        
        if (!BCrypt.Verify(model.OldPassword, user.Password))
            throw new AppException("Old password is incorrect!");
        
        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.Password))
            user.Password = BCrypt.HashPassword(model.Password);

        // copy model to user and save
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void UpdateInfo(int id, UpdateRequest model)
    {
        var user = getUser(id);
        
        if(model.Username == null)
            throw new AppException("Username invalid!");

        // copy model to user and save
        _mapper.Map(model, user);
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = getUser(id);
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    // helper methods

    private Users getUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }
    
    public void Create(CreateRequest model)
    {
        // validate
        if (_context.Users.Any(x => x.Username == model.Username))
            throw new AppException("User with the username '" + model.Username + "' already exists");

        if (model.Username == null || model.Password == null)
            throw new AppException("Username or Password invalid!");

        if(model.Password.Length<6)
            throw new AppException("Password invalid!");
        
        if (model.Password != model.ConfirmPassword)
            throw new AppException("Password or Password Confirm incorrect!");

        // map model to new user object
        var user = _mapper.Map<Users>(model);

        // hash password
        user.Password = BCrypt.HashPassword(model.Password);

        // save user
        _context.Users.Add(user);
        _context.SaveChanges();
    }

}