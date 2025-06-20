﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.SO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.SO.Mappings.Configurations
{
    public partial class ExportWarehouseConfiguration : IEntityTypeConfiguration<ExportWarehouse>
    {
        public void Configure(EntityTypeBuilder<ExportWarehouse> entity)
        {
            entity.ToTable("ExportWarehouse", "so");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.ApproveByName).HasMaxLength(150);

            entity.Property(e => e.ApproveComment).HasMaxLength(1000);

            entity.Property(e => e.ApproveDate).HasColumnType("datetime");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CreatedByName).HasMaxLength(255);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.CustomerCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CustomerName).HasMaxLength(300);

            entity.Property(e => e.DeliveryAddress).HasMaxLength(250);

            entity.Property(e => e.DeliveryCountry).HasMaxLength(250);

            entity.Property(e => e.DeliveryDistrict).HasMaxLength(250);

            entity.Property(e => e.DeliveryMethodCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.DeliveryMethodName).HasMaxLength(255);

            entity.Property(e => e.DeliveryName).HasMaxLength(250);

            entity.Property(e => e.DeliveryNote).HasMaxLength(500);

            entity.Property(e => e.DeliveryProvince).HasMaxLength(250);

            entity.Property(e => e.DeliveryWard).HasMaxLength(250);

            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.Property(e => e.EstimatedDeliveryDate).HasColumnType("datetime");

            entity.Property(e => e.File).HasColumnType("ntext");

            entity.Property(e => e.FulfillmentRequestCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Note).HasMaxLength(1000);

            entity.Property(e => e.RequestByName).HasMaxLength(255);

            entity.Property(e => e.RequestDate).HasColumnType("datetime");

            entity.Property(e => e.ShippingMethodCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ShippingMethodName).HasMaxLength(255);

            entity.Property(e => e.UpdatedByName).HasMaxLength(255);

            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.Property(e => e.WarehouseCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Kho");

            entity.Property(e => e.WarehouseId).HasComment("Kho");

            entity.Property(e => e.WarehouseName).HasMaxLength(250);

            entity.HasOne(d => d.Order)
                .WithMany(p => p.ExportWarehouse)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_ExportWarehouse_Order");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ExportWarehouse> entity);
    }
}
