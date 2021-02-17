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
            entity.ToTable("student_of_student_group");

            entity.HasIndex(e => e.StudentGroupId)
                .HasName("FK__idx");

            entity.HasIndex(e => e.StudentId)
                .HasName("FK_student_of_student_group_idx");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.StudentGroupId).HasColumnName("student_group_id");

            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.StudentGroup)
                .WithMany(p => p.StudentsOfStudentGroups)
                .HasForeignKey(d => d.StudentGroupId)
                .HasConstraintName("FK_student_group_of_student");

            entity.HasOne(d => d.Student)
                .WithMany(p => p.StudentsOfStudentGroups)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_student_of_student_group");
        }
    }
}
