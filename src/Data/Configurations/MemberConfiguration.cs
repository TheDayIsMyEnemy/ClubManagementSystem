using ClubManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClubManagementSystem.Data.Configurations
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder
                .Ignore(p => p.FullName);
            builder
                .Property(p => p.Gender)
                .HasConversion<string>();
            builder
                .Property(p => p.BirthDate)
                .HasColumnType("date");
        }
    }
}
