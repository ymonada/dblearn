using dbt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dbt.Configurations;

public class NoteConfiguration:IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasOne(s => s.user)
            .WithMany(s => s.Notes)
            .HasForeignKey(s => s.UserId);
        builder.Property(s => s.Title).IsRequired();
        builder.Property(s => s.Title).IsRequired();
        
    }
}