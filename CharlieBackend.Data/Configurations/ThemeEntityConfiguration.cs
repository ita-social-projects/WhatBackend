using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class ThemeEntityConfiguration : IEntityTypeConfiguration<Theme>
    {
        public void Configure(EntityTypeBuilder<Theme> entity)
        {
            entity.ToTable("theme");

            entity.HasIndex(e => e.Name)
                .HasName("name_UNIQUE")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasColumnType("varchar(100)")
                .HasComment("name has been set to not null and unique")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");
        }
    }
}
