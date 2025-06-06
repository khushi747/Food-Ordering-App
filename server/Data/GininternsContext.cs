﻿using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using ordermanagement.Models;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;


namespace ordermanagement.Data;

public partial class GininternsContext : DbContext
{
    public GininternsContext()
    {
    }

    public GininternsContext(DbContextOptions<GininternsContext> options)
        : base(options)
    {
    }


    //public virtual DbSet<Locker> Lockers { get; set; }

    public virtual DbSet<Menuitem> Menuitems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Orderstatus> Orderstatuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public DbSet<Users> User { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySql("server=gin-web-004;database=gininterns;user=jpatel;password=Janmesh@1234", ServerVersion.Parse("9.0.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");


        //modelBuilder.Entity<Locker>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PRIMARY");

        //    entity.ToTable("lockers");

        //    entity.HasIndex(e => e.UserId, "IX_Lockers_UserId");
        //});

        modelBuilder.Entity<Menuitem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PRIMARY");

            entity.ToTable("menuitems");

            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("orders");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.TotalPrice).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.OrderdetailId).HasName("PRIMARY");

            entity.ToTable("orderdetails");

            entity.HasIndex(e => e.OrderId, "OrderId");

            entity.Property(e => e.ItemsDetails).HasColumnType("json");
            entity.Property(e => e.TotalPrice).HasPrecision(10, 2);

            entity.HasOne(d => d.Order).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("orderdetails_ibfk_1");
        });

        modelBuilder.Entity<Orderstatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PRIMARY");

            entity.ToTable("orderstatus");

            entity.HasIndex(e => e.OrderId, "OrderId");

            entity.Property(e => e.Status).HasColumnType("enum('Pending','In Progress','Completed','Cancelled' )");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderstatuses)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("orderstatus_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
