using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class HomeworkStudentEntityConfiguration
        : IEntityTypeConfiguration<HomeworkStudent>
    {
        public void Configure(EntityTypeBuilder<HomeworkStudent> entity)
        {
            entity.ToTable("HomeworksFromStudents");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID");

            entity.Property(e => e.StudentId)
                .IsRequired()
                .HasColumnName("StudentID");

            entity.Property(e => e.HomeworkId)
                .IsRequired()
                .HasColumnName("HomeworkID");

            entity.Property(e => e.HomeworkText)
                .HasColumnName("HomeworkText")
                .HasColumnType("VARCHAR(8000)");

            entity.Property(e => e.MarkId)
                .HasColumnName("MarkId");

            entity.Property(e => e.PublishingDate)
                .IsRequired()
                .HasColumnName("PublishingDate")
                .HasColumnType("DATETIME");

            entity.Property(e => e.IsSent)
                .IsRequired()
                .HasColumnName("IsSent")
                .HasColumnType("TINYINT(1)");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(h => h.Mark)
                .WithOne(s => s.HomeworkStudent)
                .HasForeignKey<HomeworkStudent>(h => h.MarkId)
                .HasConstraintName("FK_MarkOfHomeworkFromStudent");

            entity.HasOne(h => h.Homework)
                .WithMany(h => h.HomeworkStudents)
                .HasForeignKey(h => h.HomeworkId)
                .HasConstraintName("FK_HomeworkOfStudents");

            entity.HasOne(h => h.Student)
                .WithMany(s => s.HomeworkStudents)
                .HasForeignKey(h => h.StudentId)
                .HasConstraintName("FK_StudentOfHomeworks");

            entity.HasIndex(e => new { e.StudentId, e.HomeworkId })
                .HasName("UQ_HomeworkAndStudent")
                .IsUnique();

            entity.HasIndex(e => e.HomeworkId)
                .HasName("IX_Homework");
        }
    }
}
