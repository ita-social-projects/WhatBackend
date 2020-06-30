using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.EntitiesConfiguration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("accounts");

            builder.Property(e => e.Email)
                .HasColumnType("varchar(50)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            builder.Property(e => e.FirstName)
                .HasColumnType("varchar(30)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            builder.Property(e => e.LastName)
                .HasColumnType("varchar(30)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            builder.Property(e => e.Password)
                .HasColumnType("varchar(65)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            builder.Property(e => e.Role).HasComment(@"from enum of roles: 1 - student 2 - mentor 4 - admin");

            builder.Property(e => e.Salt)
                .HasColumnType("varchar(65)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");
        }
    }
}
