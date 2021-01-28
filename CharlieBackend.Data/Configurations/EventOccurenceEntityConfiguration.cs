using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class EventOccurenceEntityConfiguration 
        : IEntityTypeConfiguration<EventOccurence>
    {
        public void Configure(EntityTypeBuilder<EventOccurence> entity)
        {
            entity.ToTable("event_occurence");

            entity.HasIndex(e => e.StudentGroupId)
                .HasName("fk_scheduled_events_student_group1");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.EventStart).HasColumnName("event_start");

            entity.Property(e => e.EventFinish).HasColumnName("event_finish");

            entity.Property(e => e.StudentGroupId).HasColumnName("student_group_id");

            entity.Property(e => e.Pattern).HasColumnName("pattern");

            entity.Property(e => e.Storage).HasColumnName("storage");

            entity.HasOne(d => d.StudentGroup)
                .WithMany(p => p.EventOccurances)
                .HasForeignKey(d => d.StudentGroupId)
                .HasConstraintName("fk_scheduled_events_student_group1");
        }
    }
}
