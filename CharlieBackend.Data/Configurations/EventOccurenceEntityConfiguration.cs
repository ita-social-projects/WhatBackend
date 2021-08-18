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
            entity.ToTable("EventOccurrences");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID")
                .HasColumnType("BIGINT UNSIGNED")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.StudentGroupId)
                .IsRequired()
                .HasColumnName("StudentGroupID")
                .HasColumnType("BIGINT UNSIGNED");

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

            entity.Property(e => e.Pattern)
                .HasColumnName("Pattern")
                .HasColumnType("TINYINT UNSIGNED")
                .HasComment("Patterns:" +
                "\n 0 - Daily," +
                "\n 1 - Weekly," +
                "\n 2 - AbsoluteMonthly," +
                "\n 3 - RelativeMonthly");

            entity.Property(e => e.Storage)
                .IsRequired()
                .HasColumnName("Storage")
                .HasColumnType("BIGINT UNSIGNED");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(d => d.StudentGroup)
                .WithMany(p => p.EventOccurances)
                .HasForeignKey(d => d.StudentGroupId)
                .HasConstraintName("FK_StudentGroupEventOccurrences");
        }
    }
}
