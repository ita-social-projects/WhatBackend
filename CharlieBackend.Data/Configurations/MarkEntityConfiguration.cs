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
            entity.ToTable("Marks");

            entity.Property(e => e.Id).HasColumnName("ID"); 

            entity.Property(e => e.Value)
                .HasColumnName("Value")
                .HasColumnType("TINYINT")
                .IsRequired(); 

            entity.Property(e => e.Comment)
                .HasColumnName("Comment")
                .HasColumnType("VARCHAR(1024)");

            entity.Property(e => e.EvaluationDate)
                .IsRequired()
                .HasColumnName("EvaluationDate")
                .HasColumnType("DATETIME");

            entity.Property(e => e.Type)
                .IsRequired()
                .HasColumnName("Type")
                .HasColumnType("TINYINT UNSIGNED")
                .HasComment("Types:\n 0 - Homework,\n 1 - Visit");

            entity.Property(e => e.EvaluatedBy)
                .IsRequired()
                .HasColumnName("EvaluatedBy")
                .HasColumnType("BIGINT UNSIGNED");

            entity.HasOne(e => e.Account)
                .WithMany(a => a.Marks)
                .HasForeignKey(d => d.EvaluatedBy)
                .HasConstraintName("FK_AccountOfMark");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");
        }
    }
}
