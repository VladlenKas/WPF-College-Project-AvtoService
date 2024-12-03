using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace AvtoService_3cursAA.Model;

public partial class Avtoservice3cursAaContext : DbContext
{
    public Avtoservice3cursAaContext()
    {
    }

    public Avtoservice3cursAaContext(DbContextOptions<Avtoservice3cursAaContext> options)
        : base(options)
    {
    }

    #region Все записи
    public virtual DbSet<Car> AllCars { get; set; } // Все автомобили

    public virtual DbSet<Carclient> AllCarclients { get; set; }

    public virtual DbSet<Checkdetail> Checkdetails { get; set; }

    public virtual DbSet<Checkprice> Checkprices { get; set; }

    public virtual DbSet<Client> AllClients { get; set; } // Все клиенты

    public virtual DbSet<Detail> AllDetails { get; set; } // Все детали

    public virtual DbSet<Employee> AllEmployees { get; set; } // Все сотрудники

    public virtual DbSet<Price> AllPrices { get; set; } // Все услуги

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Typeofrepair> Typeofrepairs { get; set; } 
    #endregion

    #region Активные записи
    public IQueryable<Car> Cars => AllCars.Where(car => !car.IsDeleted); // Только активные автомобили
    public IQueryable<Client> Clients => AllClients.Where(client => !client.IsDeleted); // Только активные клиенты
    public IQueryable<Detail> Details => AllDetails.Where(detail => !detail.IsDeleted); // Только существующие детали
    public IQueryable<Employee> Employees => AllEmployees.Where(employee => !employee.IsDeleted); // Только активные сотрудники
    public IQueryable<Price> Prices => AllPrices.Where(price => !price.IsDeleted); // Только существующие услуги
    public IQueryable<Carclient> Carclients => AllCarclients.Where(carclient => !carclient.IsDeleted); // Только существующие связи

    #endregion


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=avtoservice_3curs_aa", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.39-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.IdCar).HasName("PRIMARY");

            entity.ToTable("car");

            entity.HasIndex(e => e.IdCar, "id_car_index");

            entity.Property(e => e.IdCar).HasColumnName("id_car");
            entity.Property(e => e.Brand)
                .HasMaxLength(45)
                .HasColumnName("brand");
            entity.Property(e => e.Country)
                .HasMaxLength(45)
                .HasColumnName("country");
            entity.Property(e => e.Description)
                .HasMaxLength(45)
                .HasColumnName("description");
            entity.Property(e => e.Model)
                .HasMaxLength(45)
                .HasColumnName("model");
            entity.Property(e => e.Year)
                .HasColumnType("year")
                .HasColumnName("year");
            entity.Property(e => e.Photo).HasColumnName("photo");
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
        });

        modelBuilder.Entity<Carclient>(entity =>
        {
            entity.HasKey(e => e.IdCarclient).HasName("PRIMARY");

            entity.ToTable("carclient");

            entity.HasIndex(e => e.IdCar, "id_car_idx");

            entity.HasIndex(e => e.IdClient, "id_client_idx");

            entity.Property(e => e.IdCarclient).HasColumnName("id_carclient");
            entity.Property(e => e.IdCar).HasColumnName("id_car");
            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");

            entity.HasOne(d => d.IdCarNavigation).WithMany(p => p.Carclients)
                .HasForeignKey(d => d.IdCar)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_car");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Carclients)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_client");
        });

        modelBuilder.Entity<Checkdetail>(entity =>
        {
            entity.HasKey(e => e.IdCheckdetail).HasName("PRIMARY");

            entity.ToTable("checkdetail");

            entity.HasIndex(e => e.IdDetail, "id_detail_idx");

            entity.HasIndex(e => e.IdSale, "id_sale_idx");

            entity.Property(e => e.IdCheckdetail).HasColumnName("id_checkdetail");
            entity.Property(e => e.IdDetail).HasColumnName("id_detail");
            entity.Property(e => e.IdSale).HasColumnName("id_sale");
            entity.Property(e => e.DetailsCount).HasColumnName("details_count");

            entity.HasOne(d => d.IdDetailNavigation).WithMany(p => p.Checkdetails)
                .HasForeignKey(d => d.IdDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_checkdetail_id_detail");

            entity.HasOne(d => d.IdSaleNavigation).WithMany(p => p.Checkdetails)
                .HasForeignKey(d => d.IdSale)
                .HasConstraintName("fk_checkdetail_id_sale");
        });

        modelBuilder.Entity<Checkprice>(entity =>
        {
            entity.HasKey(e => e.IdCheckprice).HasName("PRIMARY");

            entity.ToTable("checkprice");

            entity.HasIndex(e => e.IdPrice, "id_price_idx");

            entity.HasIndex(e => e.IdSale, "id_sale_idx");

            entity.Property(e => e.IdCheckprice).HasColumnName("id_checkprice");
            entity.Property(e => e.IdPrice).HasColumnName("id_price");
            entity.Property(e => e.IdSale).HasColumnName("id_sale");

            entity.HasOne(d => d.IdPriceNavigation).WithMany(p => p.Checkprices)
                .HasForeignKey(d => d.IdPrice)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_price");

            entity.HasOne(d => d.IdSaleNavigation).WithMany(p => p.Checkprices)
                .HasForeignKey(d => d.IdSale)
                .HasConstraintName("id_sale");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("PRIMARY");

            entity.ToTable("client");

            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Firstname)
                .HasMaxLength(45)
                .HasColumnName("firstname");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(45)
                .HasColumnName("patronymic");
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .HasColumnName("phone");
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
        });

        modelBuilder.Entity<Detail>(entity =>
        {
            entity.HasKey(e => e.IdDetail).HasName("PRIMARY");

            entity.ToTable("detail");

            entity.Property(e => e.IdDetail).HasColumnName("id_detail");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Photo).HasColumnName("photo");
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.IdEmployee).HasName("PRIMARY");

            entity.ToTable("employee");

            entity.HasIndex(e => e.IdRole, "id_role_idx");

            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Firstname)
                .HasMaxLength(45)
                .HasColumnName("firstname");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Login)
                .HasMaxLength(45)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Passport)
                .HasMaxLength(10)
                .HasColumnName("passport");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(45)
                .HasColumnName("patronymic");
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .HasColumnName("phone");
            entity.Property(e => e.Seniority).HasColumnName("seniority");
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_role");
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity.HasKey(e => e.IdPrice).HasName("PRIMARY");

            entity.ToTable("price");

            entity.Property(e => e.IdPrice).HasColumnName("id_price");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Photo).HasColumnName("photo");
            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.IdSale).HasName("PRIMARY");

            entity.ToTable("sale");

            entity.HasIndex(e => e.IdCarclient, "id_carclient_idx");

            entity.HasIndex(e => e.IdEmployee, "id_employee_idx");

            entity.HasIndex(e => e.IdTypeofrepair, "id_typeofrepair_idx");

            entity.Property(e => e.IdSale).HasColumnName("id_sale");
            entity.Property(e => e.Date)
                .HasColumnType("datetime(5)")
                .HasColumnName("date");
            entity.Property(e => e.IdCarclient).HasColumnName("id_carclient");
            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");
            entity.Property(e => e.IdTypeofrepair).HasColumnName("id_typeofrepair");
            entity.Property(e => e.CostForClient).HasColumnName("cost_for_client");
            entity.Property(e => e.CostTotal).HasColumnName("cost_total");

            entity.HasOne(d => d.IdCarclientNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.IdCarclient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_carclient");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_employee");

            entity.HasOne(d => d.IdTypeofrepairNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.IdTypeofrepair)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_typeofrepair");
        });

        modelBuilder.Entity<Typeofrepair>(entity =>
        {
            entity.HasKey(e => e.IdTypeofrepair).HasName("PRIMARY");

            entity.ToTable("typeofrepair");

            entity.Property(e => e.IdTypeofrepair).HasColumnName("id_typeofrepair");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
