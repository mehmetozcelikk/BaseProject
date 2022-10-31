using BaseProject.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BaseProject.Persistence.Contexts;

public class BaseDbContext : IdentityDbContext<User,UserRole,string>
{
    protected IConfiguration Configuration { get; set; }

    //public DbSet<User> Users { get; set; }
    //public DbSet<UserRole> UserRole { get; set; }

    public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
    {
        Configuration = configuration;
    }
    public BaseDbContext(DbContextOptions options) : base(options)
    { }
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //        base.OnConfiguring(
    //            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("SomeConnectionString")));
    //}
}
