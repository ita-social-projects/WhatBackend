using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class EventOccurenceEntityConfiguration 
        : IEntityTypeConfiguration<EventOccurrence>
    {
        public void Configure(EntityTypeBuilder<EventOccurrence> entity)
        {
            entity.ToTable("event_occurence");

            entity.HasIndex(e => e.StudentGroupId)
                .HasName("FK_student_group_of_schedule");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.EventStart).HasColumnName("event_start");

            entity.Property(e => e.EventFinish).HasColumnName("event_finish");

            entity.Property(e => e.StudentGroupId).HasColumnName("student_group_id");

            entity.Property(e => e.Pattern).HasColumnName("pattern");

            entity.Property(e => e.Storage).HasColumnName("storage");

            entity.HasOne(d => d.StudentGroup)
                .WithMany(p => p.EventOccurances)
                .HasForeignKey(d => d.StudentGroupId)
                .HasConstraintName("FK_student_group_of_schedule");
        }
    }
}
