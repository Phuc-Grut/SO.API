﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.SO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.SO.Mappings.Configurations
{
    public partial class PriceListPurchaseConfiguration : IEntityTypeConfiguration<PriceListPurchase>
    {
        public void Configure(EntityTypeBuilder<PriceListPurchase> entity)
        {
            entity.ToTable("PriceListPurchase", "so");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CreatedByName).HasMaxLength(255);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.Property(e => e.Name).HasMaxLength(255);

            entity.Property(e => e.UpdatedByName).HasMaxLength(255);

            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<PriceListPurchase> entity);
    }
}
