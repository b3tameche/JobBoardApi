using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class JobTagConfiguration : IEntityTypeConfiguration<JobTag>
{
    public void Configure(EntityTypeBuilder<JobTag> builder)
    {
        builder
            .HasKey(jt => new { jt.JobId, jt.TagId });
        
        builder
            .HasOne(jt => jt.Job)
            .WithMany(j => j.JobTags)
            .HasForeignKey(jt => jt.JobId);
        
        builder
            .HasOne(jt => jt.Tag)
            .WithMany(t => t.JobTags)
            .HasForeignKey(jt => jt.TagId);
    }
}