using Microsoft.EntityFrameworkCore;
using ECommerceApi.Models;

namespace ECommerceApi;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Address> Addresses { get; set; }

    public DbSet<Order> Orders { get; set; }
public DbSet<OrderItems> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Address)
            .WithOne()
            .HasForeignKey<User>(u => u.AddressId);

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

         modelBuilder.Entity<User>(entity =>
    {
        entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
        entity.HasIndex(u => u.Name).IsUnique();
    });

      modelBuilder.Entity<Product>(entity =>
    {
        entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
        entity.Property(p => p.Price).HasPrecision(10, 2);
        entity.HasQueryFilter(p => p.Active);   
    });

     modelBuilder.Entity<CartItem>(entity =>
    {
        entity.Property(c => c.Price).HasPrecision(10, 2);
        entity.HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId);
        entity.HasOne(c => c.Product).WithMany().HasForeignKey(c => c.ProductId);
    });
    modelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasPrecision(10, 2);
    modelBuilder.Entity<OrderItems>().Property(i => i.Price).HasPrecision(10, 2);
    }
}