﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.SO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.SO.Mappings.Configurations
{
    public partial class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> entity)
        {
            entity.ToTable("Promotion", "so");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CreatedByName).HasMaxLength(255);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.EndDate).HasColumnType("date");

            entity.Property(e => e.Name).HasMaxLength(255);

            entity.Property(e => e.PromotionalCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.SalesChannel)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.Property(e => e.Stores)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.UpdatedByName).HasMaxLength(255);

            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.PromotionGroup)
                .WithMany(p => p.Promotion)
                .HasForeignKey(d => d.PromotionGroupId)
                .HasConstraintName("FK_Promotion_PromotionGroup");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Promotion> entity);
    }
}
