using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Data.Configurations
{
    class HomeworkStudentHistoryEntityConfiguration
         : IEntityTypeConfiguration<HomeworkStudentHistory>
    {
        public void Configure(EntityTypeBuilder<HomeworkStudentHistory> entity)
        {
            entity.ToTable("HomeworksFromStudentsHistory");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID");

            entity.Property(e => e.HomeworkText)
               .HasColumnName("HomeworkText")
               .HasColumnType("VARCHAR(8000)");

            entity.Property(e => e.HomeworkStudentId)
                .IsRequired()
                .HasColumnName("HomeworkFromStudentID");

            entity.Property(e => e.MarkId)
                .IsRequired()
                .HasColumnName("MarkID");

            entity.Property(e => e.PublishingDate)
                .IsRequired()
                .HasColumnName("PublishingDate")
                .HasColumnType("DATETIME");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(h => h.Mark)
                .WithOne(s => s.HomeworkStudentHistory)
                .HasForeignKey<HomeworkStudentHistory>(h => h.MarkId)
                .HasConstraintName("FK_MarkOfHitory");

            entity.HasOne(h => h.HomeworkStudent)
                .WithMany(h => h.HomeworkStudentsHistory)
                .HasForeignKey(h => h.HomeworkStudentId)
                .HasConstraintName("FK_HomeworkStudentOfHistory");
        }
    }
}
