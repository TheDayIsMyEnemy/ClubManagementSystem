using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClubManagementSystem.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        private const string adminId = "7FD40DB8-AB11-4005-AD2E-DD50D5546FA2";

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                    new IdentityRole
                    {
                        Id = adminId,
                        Name = "Administrator",
                        NormalizedName = "ADMINISTRATOR"
                    }
                );
        }
    }
}
