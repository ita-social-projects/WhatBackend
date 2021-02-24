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
            entity.ToTable("attachment_of_homework");

            entity.HasIndex(e => e.AttachmentId)
                .HasName("FK_attachment_of_homework_id");

            entity.HasIndex(e => e.HomeworkId)
                .HasName("FK_homework_of_attachment_id");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.AttachmentId).HasColumnName("attachment_id");

            entity.Property(e => e.HomeworkId).HasColumnName("homework_id");

            entity.HasOne(d => d.Attachment)
                .WithMany(p => p.AttachmentsOfHomework)
                .HasForeignKey(d => d.AttachmentId)
                .HasConstraintName("FK_attachment_of_homework");

            entity.HasOne(d => d.Homework)
                .WithMany(p => p.AttachmentsOfHomework)
                .HasForeignKey(d => d.HomeworkId)
                .HasConstraintName("FK_homework_of_attachment");
        }
    }
}
