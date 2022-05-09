using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    public class EventColorConfiguration : IEntityTypeConfiguration<EventColor>
    {
        public void Configure(EntityTypeBuilder<EventColor> builder)
        {
            builder.ToTable("eventcolors");

            builder.Property(b => b.Id).IsRequired()
                .HasColumnName("ID");

            builder.Property(b => b.Color)
              .HasColumnName("Color")
              .HasColumnType("VARCHAR(30)");

            builder.HasKey(b => b.Id)
            .HasName("PRIMARY");
        }
    }
}
