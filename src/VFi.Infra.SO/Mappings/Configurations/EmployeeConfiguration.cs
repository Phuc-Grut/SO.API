﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.SO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.SO.Mappings.Configurations
{
    public partial class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> entity)
        {
            entity.ToTable("Employee", "so");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Address).HasMaxLength(255);

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Country).HasMaxLength(50);

            entity.Property(e => e.CreatedByName).HasMaxLength(255);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.Property(e => e.District).HasMaxLength(50);

            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.GroupEmployee).HasMaxLength(255);

            entity.Property(e => e.Image).HasMaxLength(250);

            entity.Property(e => e.Name).HasMaxLength(255);

            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Province).HasMaxLength(50);

            entity.Property(e => e.TaxCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.UpdatedByName).HasMaxLength(255);

            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.Property(e => e.Ward).HasMaxLength(50);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Employee> entity);
    }
}
