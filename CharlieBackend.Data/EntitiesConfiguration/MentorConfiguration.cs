using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.EntitiesConfiguration
{
    public class MentorConfiguration : IEntityTypeConfiguration<Mentor>
    {
        public void Configure(EntityTypeBuilder<Mentor> builder)
        {
            builder.ToTable("mentors");

            builder.HasIndex(e => e.IdAccount)
                .HasName("FK_AccountOfMentor");

            builder.HasOne(d => d.IdAccountNavigation)
                .WithMany(p => p.Mentors)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK_AccountOfMentor");
        }
    }
}
