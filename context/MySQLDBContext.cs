using apidemo.Entities;
using Microsoft.EntityFrameworkCore;

namespace apidemo.Context;

public class MySQLDBContext : DbContext
{
    //public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    public DbSet<Users> Users { get; set; }
    public MySQLDBContext(DbContextOptions<MySQLDBContext> options) : base(options)
    {
        
    }
}