using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Tool_Menagement.Models;

public partial class ToolsBaseContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public ToolsBaseContext()
    {
    }

    public ToolsBaseContext(DbContextOptions<ToolsBaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Kategorium> Kategoria { get; set; }

    public virtual DbSet<Magazyn> Magazyns { get; set; }

    public virtual DbSet<NarzedziaTechnologium> NarzedziaTechnologia { get; set; }

    public virtual DbSet<Narzedzie> Narzedzies { get; set; }

    public virtual DbSet<Rejestracja> Rejestracjas { get; set; }

    public virtual DbSet<Technologium> Technologia { get; set; }

    public virtual DbSet<Zlecenie> Zlecenies { get; set; }
    public virtual DbSet<Zlecenie_TT> Zlecenie_TT { get; set; }
    public virtual DbSet<IdentityUser> IdentityUser { get; set; }
    public virtual DbSet<IdentityUser> IdentityRole { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:MyDatabase");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserName).IsRequired().HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
        });

        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);
        });

        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
        });

        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);
        });

        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ClaimType).HasMaxLength(256);
            entity.Property(e => e.ClaimValue).HasMaxLength(256);
        });

        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ClaimType).HasMaxLength(256);
            entity.Property(e => e.ClaimValue).HasMaxLength(256);
        });

        modelBuilder.Entity<Kategorium>(entity =>
        {
            entity.HasKey(e => e.IdKategorii).HasName("PK__Kategori__E2A56928B2A8F3CE");

            entity.Property(e => e.IdKategorii).HasColumnName("Id_Kategorii");
            entity.Property(e => e.MaterialWykonania).HasColumnName("Material_wykonania");
            entity.Property(e => e.Opis).HasMaxLength(32);
            entity.Property(e => e.Przeznaczenie).HasMaxLength(32);
        });

        modelBuilder.Entity<Magazyn>(entity =>
        {
            entity.HasKey(e => e.PozycjaMagazynowa).HasName("PK__Magazyn__78B0461F242BAAD7");

            entity.ToTable("Magazyn");

            entity.Property(e => e.PozycjaMagazynowa).HasColumnName("Pozycja_magazynowa");
            entity.Property(e => e.CyklRegeneracji).HasColumnName("Cykl_regeneracji");
            entity.Property(e => e.IdNarzedzia).HasColumnName("Id_narzedzia");

            entity.HasOne(d => d.IdNarzedziaNavigation).WithMany(p => p.Magazyns)
                .HasForeignKey(d => d.IdNarzedzia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rST_MagazynNarzedzie");
        });

        modelBuilder.Entity<NarzedziaTechnologium>(entity =>
        {
            entity.HasKey(e => new { e.IdNarzedzia, e.IdTechnologi }).HasName("PK__Narzedzi__6ED7F3187D2CDCAB");

            entity.ToTable("Narzedzia_Technologia");

            entity.Property(e => e.IdNarzedzia).HasColumnName("Id_narzedzia");
            entity.Property(e => e.IdTechnologi).HasColumnName("Id_technologi");
            entity.Property(e => e.CzasPracy).HasColumnName("Czas_pracy");

            entity.HasOne(d => d.IdNarzedziaNavigation).WithMany(p => p.NarzedziaTechnologia)
                .HasForeignKey(d => d.IdNarzedzia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rST_Narzedzia_TechnologiaNarzedzie");

            entity.HasOne(d => d.IdTechnologiNavigation).WithMany(p => p.NarzedziaTechnologia)
                .HasForeignKey(d => d.IdTechnologi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rST_Narzedzia_TechnologiaTechnologia");
        });

        modelBuilder.Entity<Narzedzie>(entity =>
        {
            entity.HasKey(e => e.IdNarzedzia).HasName("PK__Narzedzi__B019BBBC628096CC");

            entity.ToTable("Narzedzie");

            entity.Property(e => e.IdNarzedzia).HasColumnName("Id_narzedzia");
            entity.Property(e => e.IdKategorii).HasColumnName("Id_Kategorii");
            entity.Property(e => e.Nazwa).HasMaxLength(64);

            entity.HasOne(d => d.IdKategoriiNavigation).WithMany(p => p.Narzedzies)
                .HasForeignKey(d => d.IdKategorii)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rST_NarzedzieKategoria");
        });

        modelBuilder.Entity<Rejestracja>(entity =>
        {
            entity.HasKey(e => e.IdPozycji).HasName("PK__Rejestra__04BB3C818C437FEF");

            entity.ToTable("Rejestracja");

            entity.Property(e => e.IdPozycji).HasColumnName("Id_pozycji");
            entity.Property(e => e.DataWykonania).HasColumnName("Data_wykonania");
            entity.Property(e => e.IdZlecenia).HasColumnName("Id_zlecenia");
            entity.Property(e => e.Wykonal).HasMaxLength(64);

            entity.HasOne(d => d.IdZleceniaNavigation).WithMany(p => p.Rejestracjas)
                .HasForeignKey(d => d.IdZlecenia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rST_ZlecenieRejestracja");
        });

        modelBuilder.Entity<Technologium>(entity =>
        {
            entity.HasKey(e => e.IdTechnologi).HasName("PK__Technolo__ECE48A4CFDB3801A");

            entity.Property(e => e.IdTechnologi).HasColumnName("Id_technologi");
            entity.Property(e => e.DataUtworzenia)
                .HasColumnType("datetime")
                .HasColumnName("Data_utworzenia");
            entity.Property(e => e.Opis).HasMaxLength(100);
        });

        modelBuilder.Entity<Zlecenie>(entity =>
        {
            entity.HasKey(e => e.IdZlecenia).HasName("PK__Zlecenie__4FF1DFCEB8CCD08B");

            entity.ToTable("Zlecenie");

            entity.Property(e => e.IdZlecenia).HasColumnName("Id_zlecenia");
            entity.Property(e => e.IdTechnologi).HasColumnName("Id_technologi");

            entity.HasOne(d => d.IdTechnologiNavigation).WithMany(p => p.Zlecenies)
                .HasForeignKey(d => d.IdTechnologi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rST_ZlecenieTechnologia");
        });
        modelBuilder.Entity<Zlecenie_TT>(entity =>
        {
            entity.HasKey(e => e.IdPozycji).HasName("PK_OrderTT");

            entity.ToTable("OrderTT");

            entity.Property(e => e.IdPozycji).HasColumnName("Position_Id");
            entity.Property(e => e.IdZlecenia).HasColumnName("Order_Id");
            entity.Property(e => e.IdNarzedzia).HasColumnName("Tool_Id");
            entity.Property(e => e.Aktywne).HasColumnName("Active");

            entity.HasOne(d => d.IdZlecenieNavigation).WithMany(p => p.ZlecenieTT)
                .HasForeignKey(d => d.IdZlecenia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rST_OrderTT_Zlecenie");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
