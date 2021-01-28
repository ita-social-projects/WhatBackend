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
            entity.ToTable("scheduled_event");

            entity.Property(x => x.Id).HasColumnName("id");

            entity.Property(x => x.EventOccurenceId).HasColumnName("event_occurence_id").HasColumnType("bigint(20)");

            entity.HasOne(x => x.EventOccurence)
                .WithMany(x => x.ScheduledEvents)
                .HasForeignKey(x => x.EventOccurenceId)
                .HasConstraintName("fk_scheduled_events_event_occurence1");

            entity.Property(x => x.StudentGroupId).HasColumnName("student_group_id").HasColumnType("bigint(20)");

            entity.HasOne(x => x.StudentGroup)
                .WithMany(x => x.ScheduledEvents)
                .HasForeignKey(x => x.StudentGroupId)
                .HasConstraintName("fk_scheduled_events_student_group1");

            entity.Property(x => x.ThemeId).HasColumnName("theme_id").HasColumnType("bigint(20)");

            entity.HasOne(x => x.Theme)
                .WithMany(x => x.ScheduledEvents)
                .HasForeignKey(x => x.ThemeId)
                .HasConstraintName("fk_scheduled_events_theme1");

            entity.Property(x => x.MentorId).HasColumnName("mentor_id").HasColumnType("bigint(20)");

            entity.HasOne(x => x.Mentor)
                .WithMany(x => x.ScheduledEvents)
                .HasForeignKey(x => x.MentorId)
                .HasConstraintName("fk_scheduled_events_mentor1");

            entity.Property(x => x.LessonId).HasColumnName("lesson_id").HasColumnType("bigint(20)");

            entity.HasOne(x => x.Lesson)
                .WithOne(x => x.ScheduledEvent)
                .HasConstraintName("fk_scheduled_events_lesson1");

            entity.Property(x => x.EventStart).HasColumnName("event_start").HasColumnType("datetime");

            entity.Property(x => x.EventFinish).HasColumnName("event_finish").HasColumnType("datetime");
        }
    }
}
