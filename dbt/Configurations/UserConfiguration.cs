using Microsoft.EntityFrameworkCore;
using dbt.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dbt.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasMany(s => s.Notes)
            .WithOne(s => s.user);
        builder.Property(s => s.Login).IsRequired();
        builder.Property(s => s.Age).IsRequired();
    }
}