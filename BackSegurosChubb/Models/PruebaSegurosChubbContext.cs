using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BackSegurosChubb.Models;

public partial class PruebaSegurosChubbContext : DbContext
{
    public PruebaSegurosChubbContext()
    {
    }

    public PruebaSegurosChubbContext(DbContextOptions<PruebaSegurosChubbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Poliza> Polizas { get; set; }

    public virtual DbSet<Seguro> Seguros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {

        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdAsegurados).HasName("PK__Persona__C2D8379E97CCF92A");

            entity.ToTable("Persona");

            entity.Property(e => e.Cedula)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Poliza>(entity =>
        {
            entity.HasKey(e => e.IdPoliza).HasName("PK__Polizas__8E3943B3D70DA2E1");

            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.IdAseguradosNavigation).WithMany(p => p.Polizas)
                .HasForeignKey(d => d.IdAsegurados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Polizas__IdAsegu__5441852A");

            entity.HasOne(d => d.IdSegurosNavigation).WithMany(p => p.Polizas)
                .HasForeignKey(d => d.IdSeguros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Polizas__IdSegur__534D60F1");
        });

        modelBuilder.Entity<Seguro>(entity =>
        {
            entity.HasKey(e => e.IdSeguros).HasName("PK__Seguros__A0A46E3B47AB44C6");

            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.NombreSeguro)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
