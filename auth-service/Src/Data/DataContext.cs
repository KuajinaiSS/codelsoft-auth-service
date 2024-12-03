using auth_service.Models;
using Microsoft.EntityFrameworkCore;

namespace auth_service.Data;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    
    public DbSet<TokenBlackList> TokenBlackLists { get; set; } = null!;
    
    public DataContext(DbContextOptions options) : base(options) { }
}