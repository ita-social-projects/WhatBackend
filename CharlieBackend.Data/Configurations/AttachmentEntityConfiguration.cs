using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class AttachmentEntityConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> entity)
        {
            entity.ToTable("Attachments");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID")
                .HasColumnType("BIGINT UNSIGNED")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.CreatedOn)
                .IsRequired()
                .HasColumnName("CreatedOn")
                .HasColumnType("DATETIME")
                .HasComment("Use UTC time");

            entity.Property(e => e.CreatedByAccountId)
                .IsRequired()
                .HasColumnName("CreatedByAccountID")
                .HasColumnType("BIGINT UNSIGNED");

            entity.Property(e => e.ContainerName)
                .IsRequired()
                .HasColumnName("ContainerName")
                .HasColumnType("VARCHAR(36)")
                .HasComment("GUID length is 36 characters");

            entity.Property(e => e.FileName)
                .IsRequired()
                .HasColumnName("FileName")
                .HasColumnType("VARCHAR(100)");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasIndex(e => e.ContainerName)
                .HasName("UQ_ContainerNameAttachments")
                .IsUnique();
        }
    }
}
