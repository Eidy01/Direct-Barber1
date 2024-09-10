using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Direct_Barber.Models;

public partial class DirectBarber1Context : DbContext
{
    public DirectBarber1Context()
    {
    }

    public DirectBarber1Context(DbContextOptions<DirectBarber1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de la entidad Rol
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id);  // Definir la clave primaria

            entity.ToTable("Rol");  // Nombre de la tabla en la base de datos

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)    // Longitud máxima del campo Nombre
                .IsRequired();       // Este campo es obligatorio
        });

        // Configuración de la entidad Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);  // Definir la clave primaria

            entity.ToTable("Usuario");  // Nombre de la tabla en la base de datos

            // Configuración de las propiedades del Usuario
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");

            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .HasColumnName("apellido");

            entity.Property(e => e.Correo)
                .HasMaxLength(60)
                .IsRequired()    // El correo es obligatorio
                .HasColumnName("correo");

            entity.Property(e => e.Contrasena)
                .HasMaxLength(200)
                .IsRequired()    // La contraseña es obligatoria
                .HasColumnName("contrasena");

            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .HasColumnName("direccion");

            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");

            entity.Property(e => e.FecNacimiento)
                .HasColumnType("date")
                .HasColumnName("fec_nacimiento");

            entity.Property(e => e.FecRegistro)
                .HasDefaultValueSql("(getdate())")  // Valor por defecto
                .HasColumnType("datetime")
                .HasColumnName("fec_registro");

            entity.Property(e => e.Calificacion)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("calificacion");

            entity.Property(e => e.Foto)
                .HasColumnType("varbinary(max)")
                .HasColumnName("foto");

            entity.Property(e => e.Documento)
                .HasMaxLength(10)
                .HasColumnName("documento");

            // Relación con la tabla Rol
            entity.HasOne(u => u.Rol)                    // Relación de uno a muchos
                .WithMany(r => r.Usuarios)               // Un Rol tiene muchos Usuarios
                .HasForeignKey(u => u.Id_Rol)            // Llave foránea
                .HasConstraintName("FK_Usuario_Rol");    // Nombre de la llave foránea
        });

        // Parte parcial (en caso de tener más configuraciones)
        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
