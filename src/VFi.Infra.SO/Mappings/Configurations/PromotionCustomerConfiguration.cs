﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.SO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.SO.Mappings.Configurations
{
    public partial class PromotionCustomerConfiguration : IEntityTypeConfiguration<PromotionCustomer>
    {
        public void Configure(EntityTypeBuilder<PromotionCustomer> entity)
        {
            entity.ToTable("PromotionCustomer", "so");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.PromotionCustomer)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PromotionCustomer_Customer");

            entity.HasOne(d => d.Promotion)
                .WithMany(p => p.PromotionCustomer)
                .HasForeignKey(d => d.PromotionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PromotionCustomer_Promotion");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<PromotionCustomer> entity);
    }
}
