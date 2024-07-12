using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Warframe_Helper.Data.Models;

namespace Warframe_Helper.Data;

public partial class WarframeMarketContext : DbContext
{
    public WarframeMarketContext()
    {
    }

    public WarframeMarketContext(DbContextOptions<WarframeMarketContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alert> Alerts { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemMonitoring> ItemMonitorings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alert>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("alert_pkey");

            entity.ToTable("alert");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.TgId).HasColumnName("tg_id");

            entity.HasOne(d => d.Item).WithMany(p => p.Alerts)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_item_id");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("item_pkey");

            entity.ToTable("item");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.UrlName)
                .HasMaxLength(255)
                .HasColumnName("url_name");
        });

        modelBuilder.Entity<ItemMonitoring>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("monitoring_pkey");

            entity.ToTable("item_monitoring");

            entity.Property(e => e.ItemId)
                .ValueGeneratedNever()
                .HasColumnName("item_id");
            entity.Property(e => e.BuyCost).HasColumnName("buy_cost");
            entity.Property(e => e.SellCost).HasColumnName("sell_cost");

            entity.HasOne(d => d.Item).WithOne(p => p.ItemMonitoring)
                .HasForeignKey<ItemMonitoring>(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
