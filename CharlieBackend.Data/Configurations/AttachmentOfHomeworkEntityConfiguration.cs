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
                .HasColumnName("ID");

            entity.Property(e => e.HomeworkId)
                .IsRequired()
                .HasColumnName("HomeworkID");

            entity.Property(e => e.AttachmentId)
                .IsRequired()
                .HasColumnName("AttachmentID");

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
