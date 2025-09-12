using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Practica.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<PropertyStatus> PropertyStatuses { get; set; }

    public virtual DbSet<PropertyType> PropertyTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<TypeOfRepair> TypeOfRepairs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=79.174.88.58:16639;Database=Sotnikov;Username=Sotnikov;Password=Sotnikov123.");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("en_US.UTF-8")
            .HasPostgresExtension("pg_stat_statements");

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clients_pkey");

            entity.ToTable("clients");

            entity.HasIndex(e => e.Email, "clients_email_key").IsUnique();

            entity.HasIndex(e => e.Phone, "clients_phone_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(255)
                .HasColumnName("middle_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Clients)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("clients_role_id_fkey");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("properties_pkey");

            entity.ToTable("properties");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Area)
                .HasPrecision(10, 2)
                .HasColumnName("area");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Floor).HasColumnName("floor");
            entity.Property(e => e.PhotoPath)
                .HasColumnType("character varying")
                .HasColumnName("photo_path");
            entity.Property(e => e.Price)
                .HasPrecision(15, 2)
                .HasColumnName("price");
            entity.Property(e => e.PropertyTypeId).HasColumnName("property_type_id");
            entity.Property(e => e.RepairTypeId).HasColumnName("repair_type_id");
            entity.Property(e => e.Rooms).HasColumnName("rooms");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.TotalFloors).HasColumnName("total_floors");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.PropertyType).WithMany(p => p.Properties)
                .HasForeignKey(d => d.PropertyTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("properties_property_type_id_fkey");

            entity.HasOne(d => d.RepairType).WithMany(p => p.Properties)
                .HasForeignKey(d => d.RepairTypeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("properties_repair_type_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Properties)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("properties_status_id_fkey");
        });

        modelBuilder.Entity<PropertyStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("property_status_pkey");

            entity.ToTable("property_status");

            entity.HasIndex(e => e.StatusName, "property_status_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<PropertyType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("property_type_pkey");

            entity.ToTable("property_type");

            entity.HasIndex(e => e.TypeName, "property_type_type_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TypeName)
                .HasMaxLength(50)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.HasIndex(e => e.RoleName, "role_role_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sales_pkey");

            entity.ToTable("sales");

            entity.HasIndex(e => e.PropertyId, "sales_property_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Commission)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("commission");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.PropertyId).HasColumnName("property_id");
            entity.Property(e => e.SaleDate).HasColumnName("sale_date");
            entity.Property(e => e.SalePrice)
                .HasPrecision(15, 2)
                .HasColumnName("sale_price");

            entity.HasOne(d => d.Client).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("sales_client_id_fkey");

            entity.HasOne(d => d.Property).WithOne(p => p.Sale)
                .HasForeignKey<Sale>(d => d.PropertyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("sales_property_id_fkey");
        });

        modelBuilder.Entity<TypeOfRepair>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("type_of_repair_pkey");

            entity.ToTable("type_of_repair");

            entity.HasIndex(e => e.RepairName, "type_of_repair_repair_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RepairName)
                .HasMaxLength(50)
                .HasColumnName("repair_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
