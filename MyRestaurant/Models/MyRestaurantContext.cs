using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyRestaurant.Models;

public partial class MyRestaurantContext : DbContext
{
    public MyRestaurantContext()
    {
    }

    public MyRestaurantContext(DbContextOptions<MyRestaurantContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alergeni> Alergenis { get; set; }

    public virtual DbSet<Categorii> Categoriis { get; set; }

    public virtual DbSet<ComandaMeniu> ComandaMenius { get; set; }

    public virtual DbSet<ComandaPreparat> ComandaPreparats { get; set; }

    public virtual DbSet<Comenzi> Comenzis { get; set; }

    public virtual DbSet<Fotografi> Fotografis { get; set; }

    public virtual DbSet<MeniuPreparat> MeniuPreparats { get; set; }

    public virtual DbSet<Meniuri> Meniuris { get; set; }

    public virtual DbSet<Preparate> Preparates { get; set; }

    public virtual DbSet<Utilizatori> Utilizatoris { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=MyRestaurant;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alergeni>(entity =>
        {
            entity.HasKey(e => e.Idalergen);

            entity.ToTable("Alergeni");

            entity.Property(e => e.Idalergen).HasColumnName("IDAlergen");
            entity.Property(e => e.NumeAlergen)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Categorii>(entity =>
        {
            entity.HasKey(e => e.Idcategorie);

            entity.ToTable("Categorii");

            entity.Property(e => e.Idcategorie).HasColumnName("IDCategorie");
            entity.Property(e => e.NumeCategorie)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ComandaMeniu>(entity =>
        {
            entity.HasKey(e => new { e.Idcomanda, e.Idmeniu });

            entity.ToTable("ComandaMeniu");

            entity.Property(e => e.Idcomanda).HasColumnName("IDComanda");
            entity.Property(e => e.Idmeniu).HasColumnName("IDMeniu");

            entity.HasOne(d => d.IdcomandaNavigation).WithMany(p => p.ComandaMenius)
                .HasForeignKey(d => d.Idcomanda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComandaMeniu_Comanda");

            entity.HasOne(d => d.IdmeniuNavigation).WithMany(p => p.ComandaMenius)
                .HasForeignKey(d => d.Idmeniu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComandaMeniu_Meniu");
        });

        modelBuilder.Entity<ComandaPreparat>(entity =>
        {
            entity.HasKey(e => new { e.Idcomanda, e.Idpreparat });

            entity.ToTable("ComandaPreparat");

            entity.Property(e => e.Idcomanda).HasColumnName("IDComanda");
            entity.Property(e => e.Idpreparat).HasColumnName("IDPreparat");

            entity.HasOne(d => d.IdcomandaNavigation).WithMany(p => p.ComandaPreparats)
                .HasForeignKey(d => d.Idcomanda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComandaPreparat_Comanda");

            entity.HasOne(d => d.IdpreparatNavigation).WithMany(p => p.ComandaPreparats)
                .HasForeignKey(d => d.Idpreparat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComandaPreparat_Preparat");
        });

        modelBuilder.Entity<Comenzi>(entity =>
        {
            entity.HasKey(e => e.Idcomanda);

            entity.ToTable("Comenzi");

            entity.Property(e => e.Idcomanda).HasColumnName("IDComanda");
            entity.Property(e => e.CodUnic).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DataComanda).HasColumnType("datetime");
            entity.Property(e => e.Idutilizator).HasColumnName("IDUtilizator");
            entity.Property(e => e.Stare)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdutilizatorNavigation).WithMany(p => p.Comenzis)
                .HasForeignKey(d => d.Idutilizator)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comenzi_Utilizator");
        });

        modelBuilder.Entity<Fotografi>(entity =>
        {
            entity.HasKey(e => e.Idfotografie);

            entity.ToTable("Fotografi");

            entity.Property(e => e.Idfotografie).HasColumnName("IDFotografie");
            entity.Property(e => e.Idpreparat).HasColumnName("IDPreparat");

            entity.HasOne(d => d.IdpreparatNavigation).WithMany(p => p.Fotografis)
                .HasForeignKey(d => d.Idpreparat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fotografi_Preparate");
        });

        modelBuilder.Entity<MeniuPreparat>(entity =>
        {
            entity.HasKey(e => new { e.Idmeniu, e.Idpreparat });

            entity.ToTable("MeniuPreparat");

            entity.Property(e => e.Idmeniu).HasColumnName("IDMeniu");
            entity.Property(e => e.Idpreparat).HasColumnName("IDPreparat");

            entity.HasOne(d => d.IdmeniuNavigation).WithMany(p => p.MeniuPreparats)
                .HasForeignKey(d => d.Idmeniu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeniuPreparat_Meniu");

            entity.HasOne(d => d.IdpreparatNavigation).WithMany(p => p.MeniuPreparats)
                .HasForeignKey(d => d.Idpreparat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeniuPreparat_Preparate");
        });

        modelBuilder.Entity<Meniuri>(entity =>
        {
            entity.HasKey(e => e.Idmeniu);

            entity.ToTable("Meniuri");

            entity.Property(e => e.Idmeniu).HasColumnName("IDMeniu");
            entity.Property(e => e.Denumire)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Idcategorie).HasColumnName("IDCategorie");

            entity.HasOne(d => d.IdcategorieNavigation).WithMany(p => p.Meniuris)
                .HasForeignKey(d => d.Idcategorie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Meniuri_Categorii");
        });

        modelBuilder.Entity<Preparate>(entity =>
        {
            entity.HasKey(e => e.Idpreparat);

            entity.ToTable("Preparate");

            entity.Property(e => e.Idpreparat).HasColumnName("IDPreparat");
            entity.Property(e => e.Denumire)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Idcategorie).HasColumnName("IDCategorie");
            entity.Property(e => e.Pret).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdcategorieNavigation).WithMany(p => p.Preparates)
                .HasForeignKey(d => d.Idcategorie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Preparate_Categorii");

            entity.HasMany(d => d.Idalergens).WithMany(p => p.Idpreparats)
                .UsingEntity<Dictionary<string, object>>(
                    "PreparatAlergen",
                    r => r.HasOne<Alergeni>().WithMany()
                        .HasForeignKey("Idalergen")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PreparatAlergen_Alergeni"),
                    l => l.HasOne<Preparate>().WithMany()
                        .HasForeignKey("Idpreparat")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PreparatAlergen_Preparat"),
                    j =>
                    {
                        j.HasKey("Idpreparat", "Idalergen");
                        j.ToTable("PreparatAlergen");
                        j.IndexerProperty<int>("Idpreparat").HasColumnName("IDPreparat");
                        j.IndexerProperty<int>("Idalergen").HasColumnName("IDAlergen");
                    });
        });

        modelBuilder.Entity<Utilizatori>(entity =>
        {
            entity.HasKey(e => e.Idutilizator);

            entity.ToTable("Utilizatori");

            entity.HasIndex(e => e.Email, "UQ_User_Email").IsUnique();

            entity.Property(e => e.Idutilizator).HasColumnName("IDUtilizator");
            entity.Property(e => e.AdresaLivrare)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nume)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Parola)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Prenume)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefon)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.TipUtilizator)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
