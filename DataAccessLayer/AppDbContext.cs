using Microsoft.EntityFrameworkCore;
using UP.Core.Entities;

namespace UP.DataLayer;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<SportEntity> Sports { get; set; }
    public DbSet<EventEntity> Events { get; set; }
    public DbSet<MatchEntity> Matches { get; set; }
    public DbSet<BetEntity> Bets { get; set; }
    public DbSet<OddEntity> Odds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventEntity>()
          .HasOne(e => e.SportEntity)
          .WithMany(e => e.Events)
          .HasForeignKey(e => e.SportEntityId);

        modelBuilder.Entity<MatchEntity>()
          .HasOne(e => e.EventEntity)
          .WithMany(e => e.Matches)
          .HasForeignKey(e => e.EventEntityId);

        modelBuilder.Entity<BetEntity>()
          .HasOne(e => e.MatchEntity)
          .WithMany(e => e.Bets)
          .HasForeignKey(e => e.MatchEntityId);

        modelBuilder.Entity<OddEntity>()
           .HasOne(e => e.BetEntity)
           .WithMany(e => e.Odds)
           .HasForeignKey(e => e.BetEntityId);

        modelBuilder.Entity<OddEntity>()
            .Property(o => o.Value)
            .HasColumnType("decimal(18, 6)");

        modelBuilder.Entity<OddEntity>()
           .Property(o => o.SpecialBetValue)
           .HasColumnType("decimal(18, 6)");

        base.OnModelCreating(modelBuilder);
    }
}