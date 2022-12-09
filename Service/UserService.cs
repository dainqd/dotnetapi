using apidemo.Authorization;
using apidemo.Context;
using apidemo.Entities;
using apidemo.Models.Users;
using AutoMapper;
using BCrypt.Net;

namespace apidemo.Service;

public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    void Register(RegisterRequest model);
    IEnumerable<Users> GetAll();
    Users GetById(int id);
    void Create(CreateRequest model);
    void Update(int id, UpdateRequest model);
    void Delete(int id);
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
        if (user == null)
            throw new AppException("Username is incorrect");
        // if (user == null )
        //     throw new AppException("Username or password is incorrect");
        if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            throw new AppException("Password is incorrect");
        
        // authentication successful
        var response = _mapper.Map<AuthenticateResponse>(user);
        response.Token = _jwtUtils.GenerateToken(user);
        return response;
    }

    public void Register(RegisterRequest model)
    {
        if (_context.Users.Any(x => x.Username == model.Username))
            throw new AppException("Username '" + model.Username + "' is already taken");

        if (model.Password != model.ConfirmPassword)
            throw new AppException("Password incorrect!");
        
        // map model to new user object
        var user = _mapper.Map<Users>(model);

        // hash password
        user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

        // save user
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public IEnumerable<Users> GetAll()
    {
        return _context.Users;
    }
    
    
    public Users GetById(int id)
    {
        return getUser(id);
    }

    public void Create(CreateRequest model)
    {
        // validate
        if (_context.Users.Any(x => x.Username == model.Username))
            throw new AppException("User with the username '" + model.Username + "' already exists");

        if (model.Password != model.ConfirmPassword)
            throw new AppException("Password incorrect!");
        
        // map model to new user object
        var user = _mapper.Map<Users>(model);

        // hash password
        user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

        // save user
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(int id, UpdateRequest model)
    {
        var user = getUser(id);

        // validate
        if (model.Username != user.Username && _context.Users.Any(x => x.Username == model.Username))
            throw new AppException("User with the username '" + model.Username + "' already exists");

        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.Password))
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

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
}
