using IDemandApp.Domain.Orders;

namespace IDemandApp.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Ignore<Notification>();

        builder.Entity<Order>()
            .HasMany(o => o.Products)
            .WithMany(p => p.Orders)
            .UsingEntity(x => x.ToTable("OrderProducts"));
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder config)
    {
        config.Properties<string>()
            .HaveMaxLength(100);
    }
}
