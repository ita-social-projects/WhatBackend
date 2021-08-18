using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class ThemeEntityConfiguration : IEntityTypeConfiguration<Theme>
    {
        public void Configure(EntityTypeBuilder<Theme> entity)
        {
            entity.ToTable("Themes");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID")
                .HasColumnType("BIGINT UNSIGNED")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("VARCHAR(100)");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasIndex(e => e.Name)
                .HasName("UQ_NameThemes")
                .IsUnique();
        }
    }
}
