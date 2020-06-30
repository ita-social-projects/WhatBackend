using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.EntitiesConfiguration
{
    public class StudentsOfGroupsConfiguration : IEntityTypeConfiguration<StudentsOfGroups>
    {
        public void Configure(EntityTypeBuilder<StudentsOfGroups> builder)
        {
            builder.ToTable("studentsofgroups");

            builder.HasIndex(e => e.IdStudent)
                .HasName("FK_StudentOfGroup");

            builder.HasIndex(e => new { e.IdStudentGroup, e.IdStudent })
                .HasName("AK_UniqueStudentAndGroup")
                .IsUnique();

            builder.HasOne(d => d.IdStudentNavigation)
                .WithMany(p => p.StudentsOfGroups)
                .HasForeignKey(d => d.IdStudent)
                .HasConstraintName("FK_StudentOfGroup");

            builder.HasOne(d => d.IdStudentGroupNavigation)
                .WithMany(p => p.StudentsOfGroups)
                .HasForeignKey(d => d.IdStudentGroup)
                .HasConstraintName("FK_GroupOfStudent");
        }
    }
}
