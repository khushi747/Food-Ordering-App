//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;
//using ordermanagement.Models;
//using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

//namespace ordermanagement.Data;

//public partial class OrdermanagementdbContext1 : DbContext
//{
//    public OrdermanagementdbContext1()
//    {
//    }

//    public OrdermanagementdbContext1(DbContextOptions<OrdermanagementdbContext1> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<Menuitem> Menuitems { get; set; }

//    public virtual DbSet<Order> Orders { get; set; }

//    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

//    public virtual DbSet<Orderstatus> Orderstatuses { get; set; }
//    public object OrderStatuses { get; internal set; }
//    public virtual DbSet<User> Users { get; set; }
//    public object ViewMenuDto { get; internal set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder
//            .UseCollation("utf8mb4_0900_ai_ci")
//            .HasCharSet("utf8mb4");

//        modelBuilder.Entity<Menuitem>(entity =>
//        {
//            entity.HasKey(e => e.ItemId).HasName("PRIMARY");
//            entity.ToTable("menuitems");
//            entity.Property(e => e.Image).HasMaxLength(255);
//            entity.Property(e => e.Name).HasMaxLength(255);
//            entity.Property(e => e.Price).HasPrecision(10, 2);
//        });

//        modelBuilder.Entity<Order>(entity =>
//        {
//            entity.HasKey(e => e.OrderId).HasName("PRIMARY");
//            entity.ToTable("orders");
//            entity.HasIndex(e => e.UserId, "UserId");
//            entity.Property(e => e.OrderDate).HasColumnType("datetime");
//            entity.Property(e => e.TotalPrice).HasPrecision(10, 2);

//            entity.HasOne(d => d.User)
//                .WithMany(p => p.Orders)
//                .HasForeignKey(d => d.UserId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("orders_ibfk_1");

//            // Correctly define one-to-many relationship between Order and Orderdetails
//            entity.HasMany(d => d.Orderdetails)
//                .WithOne(d => d.Order)
//                .HasForeignKey(d => d.OrderId)
//                .OnDelete(DeleteBehavior.Cascade);  // When Order is deleted, related Orderdetails will be deleted as well
//        });

//        modelBuilder.Entity<Orderdetail>(entity =>
//        {
//            entity.HasKey(e => e.OrderdetailId).HasName("PRIMARY");
//            entity.ToTable("orderdetails");
//            entity.HasIndex(e => e.OrderId, "OrderId");

//            entity.Property(e => e.ItemsDetails).HasColumnType("json");
//            entity.Property(e => e.TotalPrice).HasPrecision(10, 2);

//            entity.HasOne(d => d.Order)
//                .WithMany(p => p.Orderdetails)
//                .HasForeignKey(d => d.OrderId)
//                .HasConstraintName("orderdetails_ibfk_1");
//        });

//        modelBuilder.Entity<Orderstatus>(entity =>
//        {
//            entity.HasKey(e => e.StatusId).HasName("PRIMARY");
//            entity.ToTable("orderstatus");
//            entity.HasIndex(e => e.OrderId, "OrderId");
//            entity.Property(e => e.Status).HasColumnType("enum('Pending','In Progress','Completed')");

//            entity.HasOne(d => d.Order)
//                .WithMany(p => p.Orderstatuses)
//                .HasForeignKey(d => d.OrderId)
//                .HasConstraintName("orderstatus_ibfk_1");
//        });

//        modelBuilder.Entity<User>(entity =>
//        {
//            entity.HasKey(e => e.UserId).HasName("PRIMARY");
//            entity.ToTable("users");
//            entity.HasIndex(e => e.Email, "Email").IsUnique();
//            entity.Property(e => e.Name).HasMaxLength(255);
//            entity.Property(e => e.Password).HasMaxLength(255);
//            entity.Property(e => e.Role).HasColumnType("enum('Admin','User','Chef')");
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }



//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
