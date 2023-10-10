#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kerial.Models;

public partial class DbKerialContext : DbContext
{
    public DbKerialContext()
    {
    }

    public DbKerialContext(DbContextOptions<DbKerialContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Depense> Depense { get; set; }

    public virtual DbSet<Utilisateur> Utilisateur { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-BVPGELS\\SQLEXPRESS;Initial Catalog=DbKerial;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Depense>(entity =>
        {
            entity.HasKey(e => e.idDepense).HasName("PK__Depense__BCBC7CF5CD391601");

            entity.Property(e => e.idDepense).ValueGeneratedNever();
            entity.Property(e => e.commentaire)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.date).HasColumnType("datetime");
            entity.Property(e => e.devise)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.nature)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(e => e.idUtilisateur).HasName("PK__Utilisat__5366DB194D8C658B");

            entity.Property(e => e.idUtilisateur).ValueGeneratedNever();
            entity.Property(e => e.devise)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.nomDeFamille)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.prenom)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}