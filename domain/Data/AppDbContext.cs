using Domain.Models;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Tackle> Tackle { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<FishSpecies> FishSpecies { get; set; }
    public DbSet<FishingReport> FishingReports { get; set; }
    public DbSet<Landing> Landings { get; set; }

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

        modelBuilder.Entity<FishSpecies>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            
            entity.Property(e => e.Order)
                .IsRequired();
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.ScientificName)
                .HasMaxLength(150);
            
            entity.Property(e => e.Description)
                .HasMaxLength(1000);
            
            entity.Property(e => e.CreatedDate)
                .IsRequired();

            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Order);

            // Seed data with South Florida inshore fish species (ordered by desirability/popularity)
            var createdDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            entity.HasData(
                new FishSpecies
                {
                    Id = 1,
                    Order = 1,
                    Name = "Common Snook",
                    ScientificName = "Centropomus undecimalis",
                    Description = "Highly prized gamefish with distinctive black lateral line. Found in mangroves, inlets, and shallow flats. Slot limit 28-33 inches.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 2,
                    Order = 2,
                    Name = "Tarpon",
                    ScientificName = "Megalops atlanticus",
                    Description = "The 'Silver King' - massive gamefish known for spectacular jumps. Catch and release only. Found in channels, flats, and backcountry.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 3,
                    Order = 3,
                    Name = "Redfish (Red Drum)",
                    ScientificName = "Sciaenops ocellatus",
                    Description = "Copper-bronze fish with distinctive black spots. Excellent table fare and strong fighter. Found on flats and in shallow water.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 4,
                    Order = 4,
                    Name = "Spotted Seatrout",
                    ScientificName = "Cynoscion nebulosus",
                    Description = "Popular inshore gamefish with dark spots. Excellent eating and fun to catch. Found over grass flats and shallow waters.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 5,
                    Order = 5,
                    Name = "Bonefish",
                    ScientificName = "Albula vulpes",
                    Description = "The 'Gray Ghost' of the flats. Extremely wary and challenging to catch. Primarily found on shallow sandy flats in the Keys.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 6,
                    Order = 6,
                    Name = "Permit",
                    ScientificName = "Trachinotus falcatus",
                    Description = "Elite gamefish of the flats. Difficult to hook and strong fighter. Found on sandy flats and around wrecks in the Keys.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 7,
                    Order = 7,
                    Name = "Mangrove Snapper",
                    ScientificName = "Lutjanus griseus",
                    Description = "Excellent table fare found around mangroves and structure. Gray to reddish coloration with two canine teeth.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 8,
                    Order = 8,
                    Name = "Jack Crevalle",
                    ScientificName = "Caranx hippos",
                    Description = "Aggressive predator and strong fighter. Bronze to yellow coloration. Found in schools throughout South Florida waters.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 9,
                    Order = 9,
                    Name = "Barracuda",
                    ScientificName = "Sphyraena barracuda",
                    Description = "Fierce predator with razor-sharp teeth. Excellent for catching on artificial lures. Found around reefs and flats.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 10,
                    Order = 10,
                    Name = "Sheepshead",
                    ScientificName = "Archosargus probatocephalus",
                    Description = "Black and white striped fish with human-like teeth. Excellent table fare. Found around structure, docks, and bridges.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 11,
                    Order = 11,
                    Name = "Black Drum",
                    ScientificName = "Pogonias cromis",
                    Description = "Large bottom-dwelling fish with barbels under chin. Found in shallow waters and around oyster bars.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 12,
                    Order = 12,
                    Name = "Flounder",
                    ScientificName = "Paralichthys albigutta",
                    Description = "Flatfish that lies on sandy bottoms. Excellent table fare. Both eyes on the same side of head when mature.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 13,
                    Order = 13,
                    Name = "Ladyfish",
                    ScientificName = "Elops saurus",
                    Description = "Acrobatic fighter that jumps when hooked. Silver coloration. Found in shallow waters and canals throughout South Florida.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 14,
                    Order = 14,
                    Name = "Spanish Mackerel",
                    ScientificName = "Scomberomorus maculatus",
                    Description = "Fast-swimming fish with yellow spots. Good table fare when fresh. Found in nearshore and inshore waters.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 15,
                    Order = 15,
                    Name = "Cobia",
                    ScientificName = "Rachycentron canadum",
                    Description = "Large brown fish often mistaken for shark. Excellent eating and strong fighter. Often follows rays and sharks.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 16,
                    Order = 16,
                    Name = "Tripletail",
                    ScientificName = "Lobotes surinamensis",
                    Description = "Unique fish that floats on its side mimicking debris. Excellent table fare. Found around floating objects and structure.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 17,
                    Order = 17,
                    Name = "Pompano",
                    ScientificName = "Trachinotus carolinus",
                    Description = "Premium table fare with silvery, laterally compressed body. Found on sandy beaches and flats.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 18,
                    Order = 18,
                    Name = "Blue Runner",
                    ScientificName = "Caranx crysos",
                    Description = "Small jack species often found in schools. Good live bait for larger fish. Blue-green coloration on top.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 19,
                    Order = 19,
                    Name = "Pinfish",
                    ScientificName = "Lagodon rhomboides",
                    Description = "Common baitfish with distinctive dark spot. Found around grass beds and structure. Often used as bait for larger fish.",
                    CreatedDate = createdDate
                },
                new FishSpecies
                {
                    Id = 20,
                    Order = 20,
                    Name = "Grunt",
                    ScientificName = "Haemulon plumieri",
                    Description = "Common reef fish that makes grunting sound. Silver with blue and yellow stripes. Found around structure and reefs.",
                    CreatedDate = createdDate
                }
            );
        });

        modelBuilder.Entity<FishingReport>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            
            entity.Property(e => e.AccountId)
                .IsRequired();
            
            entity.Property(e => e.LocationId)
                .IsRequired();
            
            entity.Property(e => e.WindDirection)
                .HasMaxLength(10);
            
            entity.Property(e => e.WeatherConditions)
                .HasConversion<string>();
            
            entity.Property(e => e.Notes)
                .HasMaxLength(2000);
            
            entity.Property(e => e.DaytimeTemperature)
                .HasPrecision(5, 2);
            
            entity.Property(e => e.WaterTemperature)
                .HasPrecision(5, 2);
            
            entity.Property(e => e.CreatedDate)
                .IsRequired();

            entity.HasQueryFilter(e => e.DeletedDate == null);

            entity.HasIndex(e => e.AccountId);
            entity.HasIndex(e => e.LocationId);
            entity.HasIndex(e => e.TripDate);
            entity.HasIndex(e => e.DeletedDate);

            entity.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Location)
                .WithMany()
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Landing>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            
            entity.Property(e => e.AccountId)
                .IsRequired();
            
            entity.Property(e => e.FishSpeciesId)
                .IsRequired();
            
            entity.Property(e => e.FishingReportId)
                .IsRequired();
            
            entity.Property(e => e.LengthInInches)
                .HasPrecision(5, 2);
            
            entity.Property(e => e.Released)
                .IsRequired()
                .HasDefaultValue(true);
            
            entity.Property(e => e.CreatedDate)
                .IsRequired();

            entity.HasQueryFilter(e => e.DeletedDate == null);

            entity.HasIndex(e => e.AccountId);
            entity.HasIndex(e => e.FishSpeciesId);
            entity.HasIndex(e => e.FishingReportId);
            entity.HasIndex(e => e.LengthInInches);
            entity.HasIndex(e => e.TimeOfCatch);
            entity.HasIndex(e => e.DeletedDate);

            entity.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.FishSpecies)
                .WithMany()
                .HasForeignKey(e => e.FishSpeciesId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.FishingReport)
                .WithMany()
                .HasForeignKey(e => e.FishingReportId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Lure)
                .WithMany()
                .HasForeignKey(e => e.LureUsed)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Rod)
                .WithMany()
                .HasForeignKey(e => e.RodUsed)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Reel)
                .WithMany()
                .HasForeignKey(e => e.ReelUsed)
                .OnDelete(DeleteBehavior.NoAction);
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

        var fishSpeciesEntries = ChangeTracker.Entries<FishSpecies>()
            .Where(e => e.State == EntityState.Added);

        foreach (var entry in fishSpeciesEntries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }
        }

        var fishingReportEntries = ChangeTracker.Entries<FishingReport>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in fishingReportEntries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }
            entry.Entity.UpdatedDate = DateTime.UtcNow;
        }

        var landingEntries = ChangeTracker.Entries<Landing>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in landingEntries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }
            entry.Entity.UpdatedDate = DateTime.UtcNow;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}