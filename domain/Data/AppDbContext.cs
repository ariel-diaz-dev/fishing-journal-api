using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Tackle> Tackle { get; set; }
    public DbSet<Location> Locations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.CreatedDate)
                .IsRequired();
            
            entity.Property(e => e.UpdatedDate)
                .IsRequired();

            entity.HasQueryFilter(e => e.DeletedDate == null);

            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.DeletedDate);
        });

        modelBuilder.Entity<Tackle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            
            entity.Property(e => e.AccountId)
                .IsRequired();
            
            entity.Property(e => e.Type)
                .IsRequired()
                .HasConversion<string>();
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Description)
                .HasMaxLength(1000);
            
            entity.Property(e => e.CreatedDate)
                .IsRequired();
            
            entity.Property(e => e.UpdatedDate)
                .IsRequired();

            entity.HasQueryFilter(e => e.DeletedDate == null);

            entity.HasIndex(e => e.AccountId);
            entity.HasIndex(e => e.DeletedDate);
            entity.HasIndex(e => e.Type);

            entity.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Latitude)
                .IsRequired()
                .HasColumnType("decimal(10,8)");
            
            entity.Property(e => e.Longitude)
                .IsRequired()
                .HasColumnType("decimal(11,8)");
            
            entity.Property(e => e.Description)
                .HasMaxLength(1000);
            
            entity.Property(e => e.Order)
                .IsRequired();
            
            entity.Property(e => e.CreatedDate)
                .IsRequired();

            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Order);

            // Seed data
            var createdDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            entity.HasData(
                new Location
                {
                    Id = 1,
                    Name = "Flamingo, Everglades National Park",
                    Latitude = 25.14127m,
                    Longitude = -80.92764m,
                    Description = "Southern headquarters of Everglades National Park, located at the end of the only road through the park from Florida City.",
                    Order = 1,
                    CreatedDate = createdDate
                },
                new Location
                {
                    Id = 2,
                    Name = "Card Sound, Florida Keys",
                    Latitude = 25.2873m,
                    Longitude = -80.3685m,
                    Description = "Fishing area near Card Sound Bridge, connecting mainland Florida to the Upper Keys.",
                    Order = 2,
                    CreatedDate = createdDate
                },
                new Location
                {
                    Id = 3,
                    Name = "Little Duck Key, Florida Keys",
                    Latitude = 24.681084m,
                    Longitude = -81.231998m,
                    Description = "Small island in the Lower Florida Keys at Mile Marker 40, west end of the Seven Mile Bridge.",
                    Order = 3,
                    CreatedDate = createdDate
                },
                new Location
                {
                    Id = 4,
                    Name = "Turkey Point, Biscayne National Park",
                    Latitude = 25.4341667m,
                    Longitude = -80.3297222m,
                    Description = "Area near Turkey Point Nuclear Plant, adjacent to Biscayne National Park in Homestead, Florida.",
                    Order = 4,
                    CreatedDate = createdDate
                }
            );
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var accountEntries = ChangeTracker.Entries<Account>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in accountEntries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }
            entry.Entity.UpdatedDate = DateTime.UtcNow;
        }

        var tackleEntries = ChangeTracker.Entries<Tackle>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in tackleEntries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }
            entry.Entity.UpdatedDate = DateTime.UtcNow;
        }

        var locationEntries = ChangeTracker.Entries<Location>()
            .Where(e => e.State == EntityState.Added);

        foreach (var entry in locationEntries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}