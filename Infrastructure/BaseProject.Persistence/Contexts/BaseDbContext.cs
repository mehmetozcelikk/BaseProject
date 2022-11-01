using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BaseProject.Persistence.Contexts;

public class BaseDbContext : DbContext
{





    public BaseDbContext(DbContextOptions options) : base(options)
    { }





    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //        base.OnConfiguring(
    //            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("SomeConnectionString")));
    //}
}
