﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using VFi.Domain.SO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace VFi.Infra.SO.Mappings.Configurations
{
    public partial class QuotationAttachmentConfiguration : IEntityTypeConfiguration<QuotationAttachment>
    {
        public void Configure(EntityTypeBuilder<QuotationAttachment> entity)
        {
            entity.ToTable("QuotationAttachment", "so");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.AttachType).HasMaxLength(20);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.DisplayOrder).HasDefaultValueSql("((0))");

            entity.Property(e => e.Name).HasMaxLength(400);

            entity.Property(e => e.Path).HasMaxLength(255);

            entity.HasOne(d => d.Quotation)
                .WithMany(p => p.QuotationAttachment)
                .HasForeignKey(d => d.QuotationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuotationAttachment_Quotation");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<QuotationAttachment> entity);
    }
}
