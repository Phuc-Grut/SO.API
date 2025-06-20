﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.SO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.SO.Mappings.Configurations
{
    public partial class LeadProfileConfiguration : IEntityTypeConfiguration<LeadProfile>
    {
        public void Configure(EntityTypeBuilder<LeadProfile> entity)
        {
            entity.ToTable("LeadProfile", "so");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedByName).HasMaxLength(255);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Key)
            .HasMaxLength(50)
            .IsUnicode(false);
            entity.Property(e => e.UpdatedByName).HasMaxLength(255);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Value).HasMaxLength(500);

            entity.HasOne(d => d.Lead).WithMany(p => p.LeadProfiles)
            .HasForeignKey(d => d.LeadId)
            .HasConstraintName("FK_LeadProfile_Lead");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<LeadProfile> entity);
    }
}
