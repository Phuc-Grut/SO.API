﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.SO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.SO.Mappings.Configurations
{
    public partial class ReturnOrderConfiguration : IEntityTypeConfiguration<ReturnOrder>
    {
        public void Configure(EntityTypeBuilder<ReturnOrder> entity)
        {
            entity.ToTable("ReturnOrder", "so");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.AccountName).HasMaxLength(255);

            entity.Property(e => e.AmountDiscount).HasColumnType("money");

            entity.Property(e => e.ApproveByName).HasMaxLength(250);

            entity.Property(e => e.ApproveComment).HasMaxLength(1000);

            entity.Property(e => e.ApproveDate).HasColumnType("datetime");

            entity.Property(e => e.Calculation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('*')");

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CreatedByName).HasMaxLength(255);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CurrencyName).HasMaxLength(255);

            entity.Property(e => e.CustomerCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CustomerName).HasMaxLength(300);

            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18, 10)");

            entity.Property(e => e.File).HasColumnType("ntext");

            entity.Property(e => e.OrderCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ReturnDate).HasColumnType("datetime");

            entity.Property(e => e.UpdatedByName).HasMaxLength(255);

            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.Property(e => e.WarehouseCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Kho");

            entity.Property(e => e.WarehouseId).HasComment("Kho");

            entity.Property(e => e.WarehouseName).HasMaxLength(250);

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.ReturnOrder)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_ReturnOrder_Customer");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ReturnOrder> entity);
    }
}
