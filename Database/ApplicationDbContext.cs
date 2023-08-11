using update.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using update.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace update.Database;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<JobTag> JobTags { get; set; }

    private readonly IConfiguration _configuration;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        this._configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            connectionString: _configuration.GetConnectionString("default"),
            options => options.EnableRetryOnFailure());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new JobTagConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());

        modelBuilder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole { Name = "Issuer", NormalizedName = "ISSUER" },
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" }
            );
    }
}