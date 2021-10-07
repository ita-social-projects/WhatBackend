using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Data.Configurations
{
    class JobMappingEntityConfiguration : IEntityTypeConfiguration<JobMapping>
    {
        public void Configure(EntityTypeBuilder<JobMapping> entity)
        {
            entity.ToTable("JobMappings");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID");

            entity.Property(e => e.CustomJobID)
                .IsRequired()
                .HasColumnName("CustomJobID")
                .HasColumnType("VARCHAR(50)");

            entity.Property(e => e.HangfireJobID)
                .IsRequired()
                .HasColumnName("HangfireJobID")
                .HasColumnType("VARCHAR(1000)");
        }
    }
}
