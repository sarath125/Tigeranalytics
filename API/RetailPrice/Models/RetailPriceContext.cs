using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RetailPrice.Models;

public partial class RetailPriceContext : DbContext
{
    public RetailPriceContext()
    {
    }

    public RetailPriceContext(DbContextOptions<RetailPriceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Pricing> Pricings { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

      

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pricing>(entity =>
        {
            entity.HasKey(e => e.PricingId).HasName("PK__Pricing__EC306B72695DED58");

            entity.ToTable("Pricing");

            entity.Property(e => e.PricingId).HasColumnName("PricingID");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Sku).HasColumnName("SKU");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");

            entity.HasOne(d => d.SkuNavigation).WithMany(p => p.Pricings)
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pricing__SKU__4BAC3F29");

            entity.HasOne(d => d.Store).WithMany(p => p.Pricings)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pricing__StoreID__4AB81AF0");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Sku).HasName("PK__Products__CA1ECF0CA3EDAA8B");

            entity.Property(e => e.Sku)
                .ValueGeneratedNever()
                .HasColumnName("SKU");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__47DBAE45");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__ProductC__19093A2B22A7FB4C");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("PK__Stores__3B82F0E1790639BD");

            entity.Property(e => e.StoreId)
                .ValueGeneratedNever()
                .HasColumnName("StoreID");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StoreName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ZipCode)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
