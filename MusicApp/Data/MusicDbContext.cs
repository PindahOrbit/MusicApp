using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MusicApp.Models;

namespace MusicApp.Data;

public partial class MusicDbContext : DbContext
{
    public MusicDbContext()
    {
    }

    public MusicDbContext(DbContextOptions<MusicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CopyrightMatch> CopyrightMatches { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleClaim> RoleClaims { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserClaim> UserClaims { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=musicConn");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CopyrightMatch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Copyrigh__3213E83F88AF0879");

            entity.Property(e => e.DetectedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.MatchedTrack).WithMany(p => p.CopyrightMatchMatchedTracks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Copyright__match__5812160E");

            entity.HasOne(d => d.Track).WithMany(p => p.CopyrightMatchTracks).HasConstraintName("FK__Copyright__track__571DF1D5");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Messages__3213E83F39DB1815");

            entity.Property(e => e.SentAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Receiver).WithMany(p => p.MessageReceivers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__receiv__6383C8BA");

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__sender__628FA481");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3213E83F38632F7A");

            entity.Property(e => e.RequestDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.RequestingArtist).WithMany(p => p.Permissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Permissio__reque__5DCAEF64");

            entity.HasOne(d => d.Track).WithMany(p => p.Permissions).HasConstraintName("FK__Permissio__track__5EBF139D");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tracks__3213E83F710E76B0");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Tracks).HasConstraintName("FK__Tracks__user_id__4D94879B");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3213E83F822469DB");

            entity.Property(e => e.PaymentDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.Payer).WithMany(p => p.TransactionPayers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__payer__693CA210");

            entity.HasOne(d => d.Receiver).WithMany(p => p.TransactionReceivers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__recei__6A30C649");

            entity.HasOne(d => d.Track).WithMany(p => p.Transactions).HasConstraintName("FK__Transacti__track__6B24EA82");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("UserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_UserRoles_RoleId");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
