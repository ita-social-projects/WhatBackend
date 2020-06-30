using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.EntitiesConfiguration
{
    public class StudentGroupConfiguration : IEntityTypeConfiguration<StudentGroup>
    {
        public void Configure(EntityTypeBuilder<StudentGroup> builder)
        {
            builder.ToTable("studentgroups");

            builder.HasIndex(e => e.IdCourse)
                .HasName("FK_CourseOfStudentGroup");

            builder.Property(e => e.FinishDate).HasColumnType("date");

            builder.Property(e => e.Name)
                .HasColumnType("varchar(100)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            builder.Property(e => e.StartDate).HasColumnType("date");

            builder.HasOne(d => d.IdCourseNavigation)
                .WithMany(p => p.StudentGroups)
                .HasForeignKey(d => d.IdCourse)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_CourseOfStudentGroup");
        }
    }
}
