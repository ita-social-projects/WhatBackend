using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.EntitiesConfiguration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("students");

            builder.HasIndex(e => e.IdAccount)
                .HasName("FK_AccountOfStudent");

            builder.HasOne(d => d.IdAccountNavigation)
                .WithMany(p => p.Students)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK_AccountOfStudent");
        }
    }
}
