﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.SO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.SO.Mappings.Configurations
{
    public partial class CustomerGroupMappingConfiguration : IEntityTypeConfiguration<CustomerGroupMapping>
    {
        public void Configure(EntityTypeBuilder<CustomerGroupMapping> entity)
        {
            entity.ToTable("CustomerGroup_Mapping", "so");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.CreatedByName).HasMaxLength(255);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.CustomerGroup)
                .WithMany(p => p.CustomerGroupMapping)
                .HasForeignKey(d => d.CustomerGroupId)
                .HasConstraintName("FK_CustomerGroup_Mapping_CustomerGroup");

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.CustomerGroupMapping)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_CustomerGroup_Mapping_Customer");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<CustomerGroupMapping> entity);
    }
}
