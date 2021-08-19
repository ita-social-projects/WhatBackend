using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class HomeworkEntityConfiguration : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> entity)
        {
            entity.ToTable("homework");

            entity.HasIndex(e =>
                new { e.LessonId })
                .HasName("FK_lesson_for_homework");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.DueDate)
                .HasColumnName("due_date")
                .HasDefaultValue(null);

            entity.Property(e => e.TaskText)
                .HasColumnName("task_text")
                .HasColumnType("varchar(4000)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            entity.Property(e => e.PublishingDate)
                .HasColumnName("publishing_date");

            entity.Property(e => e.LessonId).HasColumnName("lesson_id");

            entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .HasColumnType("BIGINT")
                .IsRequired();

            entity.HasOne(d => d.Lesson)
                .WithMany(p => p.Homeworks)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("FK_lesson_of_homework");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.Homeworks)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_account_of_homework");
        }
    }
}
