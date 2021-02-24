using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class StudentGroupEntityConfiguration : IEntityTypeConfiguration<StudentGroup>
    {
        public void Configure(EntityTypeBuilder<StudentGroup> entity)
        {
            entity.ToTable("student_group");

            entity.HasIndex(e => e.CourseId)
                .HasName("FK_course_of_student_group");

            entity.HasIndex(e => e.Name)
                .HasName("name_UNIQUE")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.CourseId).HasColumnName("course_id");

            entity.Property(e => e.FinishDate)
                .HasColumnName("finish_date")
                .HasColumnType("date");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasColumnType("varchar(100)")
                .HasComment("name has been set to not null and unique")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            entity.Property(e => e.StartDate)
                .HasColumnName("start_date")
                .HasColumnType("date");

            entity.HasOne(d => d.Course)
                .WithMany(p => p.StudentGroup)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_course_of_student_group");
        }
    }
}
