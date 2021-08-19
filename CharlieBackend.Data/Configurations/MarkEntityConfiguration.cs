using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Data.Configurations
{
    class MarkEntityConfiguration : IEntityTypeConfiguration<Mark>
    {
        public void Configure(EntityTypeBuilder<Mark> entity)
        {
            entity.ToTable("mark");

            entity.Property(e => e.Id).HasColumnName("id"); 

            entity.Property(e => e.Value)
                .HasColumnName("value")
                .HasColumnType("TINYINT")
                .IsRequired(); 

            entity.Property(e => e.Comment)
                .HasColumnName("comment")
                .HasColumnType("varchar(1024)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            entity.Property(e => e.EvaluationDate)
                .HasColumnName("evaluation_date")
                .HasColumnType("DATETIME")
                .IsRequired();

            entity.Property(e => e.Type)
                .HasColumnName("type")
                .HasColumnType("TINYINT")
                .IsRequired()
                .HasComment("Types:\n 0 - Homework,\n 1 - Visit");

            entity.Property(e => e.EvaluatedBy)
                .HasColumnName("evaluated_by")
                .HasColumnType("BIGINT")
                .IsRequired();

            entity.HasOne(e => e.Account)
               .WithMany(a => a.Marks)
               .HasForeignKey(d => d.EvaluatedBy)
               .HasConstraintName("FK_account_of_mark");
        }
    }
}
