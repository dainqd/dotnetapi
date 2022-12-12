using System.Text;
using System.Text.Json.Serialization;
using apidemo.Authorization;
using apidemo.Context;
using apidemo.Entities;
using apidemo.Hepper;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

const string Admin = nameof(Role.Admin);
const string User = nameof(Role.User);

{
    var services = builder.Services;
    var env = builder.Environment;
// Add services to the container.
    services.AddDbContext<MySQLDBContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("connectionString");
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    });
    
    services.AddCors();
    
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("d39b71f7-ec19-4b31-a20d-73b90e70c8c9")),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });
    
    services.AddAuthorization(options =>
    {
        options.AddPolicy("Admin", policy => policy.RequireRole(Admin));
        options.AddPolicy("User", policy => policy.RequireRole(User, Admin));
    });
    
    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // ignore omitted parameters on models to enable optional params (e.g. User update)
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// configure DI for application services
    services.AddScoped<IJwtUtils, JwtUtils>();

// configure DI for application services
    services.AddScoped<IUserService, UserService>();
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();