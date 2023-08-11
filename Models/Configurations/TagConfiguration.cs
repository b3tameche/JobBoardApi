using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using update.Models.Domain;

namespace update.Models.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder
            .HasIndex(each => each.TagName)
            .IsUnique();

        builder
            .HasData(
                new Tag { Id = -1, TagName = "React" },
                new Tag { Id = -2, TagName = "Angular" }
            );
    }
}