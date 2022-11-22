using BaseProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Persistence.Contexts;

public class BaseDbContext : DbContext
{
    //IConfiguration configuration;
    public BaseDbContext()
    {

    }

    public DbSet<EmailAuthenticator> EmailAuthenticators { get; set; }

    public DbSet<OperationClaim> OperationClaims { get; set; }
    public DbSet<OtpAuthenticator> OtpAuthenticators { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserOperationClaim> UserOperationClaims { get; set; }



    public BaseDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //if (!optionsBuilder.IsConfigured)
        //    base.OnConfiguring(
        //        optionsBuilder.UseNpgsql(options => options.UseNpgsql(
        //                                             configuration.GetConnectionString("SqlConnectionString"))));

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


    }

        //public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        //{
        //    Configuration = configuration;
        //}





        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //        base.OnConfiguring(
        //            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("SomeConnectionString")));
        //}
    }
