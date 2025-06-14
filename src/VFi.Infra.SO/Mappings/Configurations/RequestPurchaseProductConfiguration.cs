﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.SO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.SO.Mappings.Configurations
{
    public partial class RequestPurchaseProductConfiguration : IEntityTypeConfiguration<RequestPurchaseProduct>
    {
        public void Configure(EntityTypeBuilder<RequestPurchaseProduct> entity)
        {
            entity.ToTable("RequestPurchaseProduct", "so");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.DeliveryDate).HasColumnType("date");

            entity.Property(e => e.DisplayOrder).HasComment("Thứ tự");

            entity.Property(e => e.Note).HasMaxLength(255);

            entity.Property(e => e.Origin).HasMaxLength(250);

            entity.Property(e => e.ProductCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ProductImage).HasMaxLength(250);

            entity.Property(e => e.ProductName).HasMaxLength(250);

            entity.Property(e => e.QuantityApproved)
                .HasDefaultValueSql("((0))")
                .HasComment("Số lượng duyệt");

            entity.Property(e => e.QuantityRequest).HasDefaultValueSql("((0))");

            entity.Property(e => e.UnitCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.UnitName).HasMaxLength(50);

            entity.Property(e => e.UnitPrice).HasColumnType("money");

            entity.Property(e => e.UnitType)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.VendorCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Mã nhà cung cấp");

            entity.Property(e => e.VendorName)
                .HasMaxLength(250)
                .HasComment("Tên nhà cung cấp");

            entity.HasOne(d => d.OrderProduct)
                .WithMany(p => p.RequestPurchaseProduct)
                .HasForeignKey(d => d.OrderProductId)
                .HasConstraintName("FK_RequestPurchaseProduct_OrderProduct");

            entity.HasOne(d => d.RequestPurchase)
                .WithMany(p => p.RequestPurchaseProduct)
                .HasForeignKey(d => d.RequestPurchaseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RequestPurchaseProduct_RequestPurchase");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<RequestPurchaseProduct> entity);
    }
}
