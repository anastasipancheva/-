using Back.Models;
using Microsoft.EntityFrameworkCore;

namespace Back;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
       public DbSet<User> Users { get; set; }
       public DbSet<Inspection> Inspections { get; set; }
}