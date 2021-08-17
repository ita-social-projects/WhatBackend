using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class HomeworkEntityConfiguration : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> entity)
        {
            entity.ToTable("Homeworks");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID")
                .HasColumnType("BIGINT UNSIGNED")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.DueDate)
                .IsRequired()
                .HasColumnName("DueDate")
                .HasColumnType("DATETIME")
                .HasComment("Use UTC time");

            entity.Property(e => e.TaskText)
                .IsRequired()
                .HasColumnName("TaskText")
                .HasColumnType("VARCHAR(8000)");

            entity.Property(e => e.LessonId)
                .IsRequired()
                .HasColumnName("LessonID")
                .HasColumnType("BIGINT UNSIGNED");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(d => d.Lesson)
                .WithMany(p => p.Homeworks)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("FK_LessonHomeworks");

            entity.HasIndex(e => e.LessonId)
                .HasName("IX_Lesson");
        }
    }
}
