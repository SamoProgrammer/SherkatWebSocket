using Microsoft.EntityFrameworkCore;
using SherkatWebSocket.Entites;

namespace SherkatWebSocket.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<Video> Videos { get; set; }
}