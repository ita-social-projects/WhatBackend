using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class AttachmentEntityConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> entity)
        {
            entity.ToTable("Attachment");

            entity.HasIndex(e => e.ContainerName)
                .HasName("container_name_UNIQUE")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.CreatedOn)
                .IsRequired()
                .HasColumnName("created_on")
                .HasColumnType("datetime")
                .HasComment("created_on has been set to not null");

            entity.Property(e => e.CreatedByAccountId)
                .IsRequired()
                .HasColumnName("created_by_account_id");

            entity.Property(e => e.ContainerName)
                .IsRequired()
                .HasColumnName("container_name")
                .HasColumnType("varchar(100)")
                .HasComment("container_name has been set to not null and unique")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            entity.Property(e => e.FileName)
                .IsRequired()
                .HasColumnName("file_name")
                .HasColumnType("varchar(100)")
                .HasComment("file_name has been set to not null")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");
        }
    }
}
