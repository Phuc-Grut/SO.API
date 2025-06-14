﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.SO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.SO.Mappings.Configurations
{
    public partial class ExportWarehouseProductConfiguration : IEntityTypeConfiguration<ExportWarehouseProduct>
    {
        public void Configure(EntityTypeBuilder<ExportWarehouseProduct> entity)
        {
            entity.ToTable("ExportWarehouseProduct", "so");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DisplayOrder).HasComment("Thứ tự");
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.ProductCode)
            .HasMaxLength(50)
            .IsUnicode(false);
            entity.Property(e => e.ProductImage).HasMaxLength(250);
            entity.Property(e => e.ProductName).HasMaxLength(250);
            entity.Property(e => e.QuantityRequest).HasDefaultValueSql("((0))");
            entity.Property(e => e.UnitCode)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasComment("Đơn vị tính");
            entity.Property(e => e.UnitName).HasMaxLength(50);
            entity.Property(e => e.WarehouseCode)
            .HasMaxLength(50)
            .IsUnicode(false);
            entity.Property(e => e.WarehouseName).HasMaxLength(255);

            entity.HasOne(d => d.ExportWarehouse).WithMany(p => p.ExportWarehouseProduct)
            .HasForeignKey(d => d.ExportWarehouseId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_ExportWarehouseProduct_ExportWarehouse");

            entity.HasOne(d => d.OrderProduct).WithMany(p => p.ExportWarehouseProduct)
            .HasForeignKey(d => d.OrderProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_ExportWarehouseProduct_OrderProduct");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ExportWarehouseProduct> entity);
    }
}
