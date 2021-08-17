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
                .HasColumnName("ID")
                .HasColumnType("BIGINT UNSIGNED")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.StudentId)
                .IsRequired()
                .HasColumnName("StudentID")
                .HasColumnType("BIGINT UNSIGNED");

            entity.Property(e => e.HomeworkId)
                .IsRequired()
                .HasColumnName("HomeworkID")
                .HasColumnType("BIGINT UNSIGNED");

            entity.Property(e => e.HomeworkText)
                .HasColumnName("HomeworkText")
                .HasColumnType("VARCHAR(8000)");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(h => h.Homework)
                .WithMany(h => h.HomeworkStudents)
                .HasForeignKey(h => h.HomeworkId)
                .HasConstraintName("FK_StudentOfHomeworks");

            entity.HasOne(h => h.Student)
                .WithMany(s => s.HomeworkStudents)
                .HasForeignKey(h => h.StudentId)
                .HasConstraintName("FK_HomeworkOfStudents");

            entity.HasIndex(e => new { e.StudentId, e.HomeworkId })
                .HasName("UQ_HomeworkAndStudent")
                .IsUnique();

            entity.HasIndex(e => e.HomeworkId)
                .HasName("IX_Homework");
        }
    }
}
