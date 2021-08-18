using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class StudentOfStudentGroupEntityConfiguration
        : IEntityTypeConfiguration<StudentOfStudentGroup>
    {
        public void Configure(EntityTypeBuilder<StudentOfStudentGroup> entity)
        {
            entity.ToTable("StudentsOfStudentGroups");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID")
                .HasColumnType("BIGINT UNSIGNED")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.StudentGroupId)
                .IsRequired()
                .HasColumnName("StudentGroupID")
                .HasColumnType("BIGINT UNSIGNED");

            entity.Property(e => e.StudentId)
                .IsRequired()
                .HasColumnName("StudentID")
                .HasColumnType("BIGINT UNSIGNED");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(d => d.StudentGroup)
                .WithMany(p => p.StudentsOfStudentGroups)
                .HasForeignKey(d => d.StudentGroupId)
                .HasConstraintName("FK_StudentGroupOfStudents");

            entity.HasOne(d => d.Student)
                .WithMany(p => p.StudentsOfStudentGroups)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_StudentOfStudentGroups");

            entity.HasIndex(e => new { e.StudentId, e.StudentGroupId })
                .HasName("UQ_StudentAndStudentGroup")
                .IsUnique();
        }
    }
}
