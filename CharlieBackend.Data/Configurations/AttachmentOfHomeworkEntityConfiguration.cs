using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class AttachmentOfHomeworkEntityConfiguration
        : IEntityTypeConfiguration<AttachmentOfHomework>
    {
        public void Configure(EntityTypeBuilder<AttachmentOfHomework> entity)
        {
            entity.ToTable("AttachmentsOfHomeworks");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID")
                .HasColumnType("BIGINT UNSIGNED")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.HomeworkId)
                .IsRequired()
                .HasColumnName("HomeworkID")
                .HasColumnType("BIGINT UNSIGNED");

            entity.Property(e => e.AttachmentId)
                .IsRequired()
                .HasColumnName("AttachmentID")
                .HasColumnType("BIGINT UNSIGNED");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(d => d.Attachment)
                .WithMany(p => p.AttachmentsOfHomework)
                .HasForeignKey(d => d.AttachmentId)
                .HasConstraintName("FK_AttachmentOfHomeworks");

            entity.HasOne(d => d.Homework)
                .WithMany(p => p.AttachmentsOfHomework)
                .HasForeignKey(d => d.HomeworkId)
                .HasConstraintName("FK_HomeworkOfAttachments");

            entity.HasIndex(e => new { e.HomeworkId, e.AttachmentId })
                .HasName("UQ_AttachmentAndHomework")
                .IsUnique();
        }
    }
}
