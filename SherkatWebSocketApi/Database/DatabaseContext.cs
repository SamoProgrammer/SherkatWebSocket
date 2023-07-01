using Microsoft.EntityFrameworkCore;
using SherkatWebSocketApi.Authentication;
using SherkatWebSocketApi.Authentication.Entities;
using SherkatWebSocketApi.Entities;

namespace SherkatWebSocketApi.Database;

public class DatabaseContext:DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DatabaseContext(DbContextOptions<DatabaseContext> options):base(options)
    {
        
    }
}