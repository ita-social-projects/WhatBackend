using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class ScheduledEventEntityConfiguration
        : IEntityTypeConfiguration<ScheduledEvent>
    {
        public void Configure(EntityTypeBuilder<ScheduledEvent> entity)
        {
            entity.ToTable("ScheduledEvents");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID");

            entity.Property(e => e.EventOccurrenceId)
                .HasColumnName("EventOccurrenceID");

            entity.Property(e => e.StudentGroupId)
                .IsRequired()
                .HasColumnName("StudentGroupID");

            entity.Property(e => e.ThemeId)
                .IsRequired()
                .HasColumnName("ThemeID");

            entity.Property(e => e.MentorId)
                .IsRequired()
                .HasColumnName("MentorID");

            entity.Property(e => e.LessonId)
                .HasColumnName("LessonID");

            entity.Property(e => e.EventStart)
                .IsRequired()
                .HasColumnName("EventStart")
                .HasColumnType("DATETIME")
                .HasComment("Use UTC time");

            entity.Property(e => e.EventFinish)
                .IsRequired()
                .HasColumnName("EventFinish")
                .HasColumnType("DATETIME")
                .HasComment("Use UTC time");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(x => x.EventOccurrence)
                .WithMany(x => x.ScheduledEvents)
                .HasForeignKey(x => x.EventOccurrenceId)
                .HasConstraintName("FK_EventOccurrenceScheduledEvents");

            entity.HasOne(x => x.Lesson)
                .WithOne(x => x.ScheduledEvent)
                .HasForeignKey<ScheduledEvent>(x => x.LessonId)
                .HasConstraintName("FK_LessonScheduledEvents");

            entity.HasOne(x => x.Mentor)
                .WithMany(x => x.ScheduledEvents)
                .HasForeignKey(x => x.MentorId)
                .HasConstraintName("FK_MentorScheduledEvents");

            entity.HasOne(x => x.StudentGroup)
                .WithMany(x => x.ScheduledEvents)
                .HasForeignKey(x => x.StudentGroupId)
                .HasConstraintName("FK_StudentGroupScheduledEvents");

            entity.HasOne(x => x.Theme)
                .WithMany(x => x.ScheduledEvents)
                .HasForeignKey(x => x.ThemeId)
                .HasConstraintName("FK_ThemeScheduledEvents");

            entity.HasIndex(e => e.LessonId)
                .HasName("UQ_LessonScheduledEvents")
                .IsUnique();
        }
    }
}
