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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=localhost; database=PruebaSegurosChubb; integrated security=true; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdAsegurados).HasName("PK__Persona__0B3E9C99BB9740EC");

            entity.ToTable("Persona");

            entity.Property(e => e.IdAsegurados).HasColumnName("Id_Asegurados");
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
            entity.HasKey(e => e.IdPoliza).HasName("PK__Polizas__A93FD36DDBC89EAC");

            entity.Property(e => e.IdPoliza).HasColumnName("Id_Poliza");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.IdAsegurados).HasColumnName("Id_Asegurados");
            entity.Property(e => e.IdSeguros).HasColumnName("Id_Seguros");

            entity.HasOne(d => d.IdAseguradosNavigation).WithMany(p => p.Polizas)
                .HasForeignKey(d => d.IdAsegurados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Polizas__Id_Aseg__3C69FB99");

            entity.HasOne(d => d.IdSegurosNavigation).WithMany(p => p.Polizas)
                .HasForeignKey(d => d.IdSeguros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Polizas__Id_Segu__3B75D760");
        });

        modelBuilder.Entity<Seguro>(entity =>
        {
            entity.HasKey(e => e.IdSeguros).HasName("PK__Seguros__EEC37FDAE6843247");

            entity.Property(e => e.IdSeguros).HasColumnName("Id_Seguros");
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
