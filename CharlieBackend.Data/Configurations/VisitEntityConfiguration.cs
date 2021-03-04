using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class VisitEntityConfiguration : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> entity)
        {
            entity.ToTable("visit");

            entity.HasIndex(e => e.LessonId)
                .HasName("FK_lesson_of_visit");

            entity.HasIndex(e => e.StudentId)
                .HasName("FK_student_of_visit_idx");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Comment)
                .HasColumnName("comment")
                .HasColumnType("varchar(1024)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            entity.Property(e => e.LessonId).HasColumnName("lesson_id");

            entity.Property(e => e.Presence)
                .HasColumnName("presence")
                .HasComment("presence default value has been set");

            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.Property(e => e.StudentMark).HasColumnName("student_mark");

            entity.HasOne(d => d.Lesson)
                .WithMany(p => p.Visits)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("FK_lesson_of_visit");

            entity.HasOne(d => d.Student)
                .WithMany(p => p.Visits)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_student_of_visit");
        }
    }
}
