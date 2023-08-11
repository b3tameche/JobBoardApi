using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace update.Models.Domain;

public class Job
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public User User { get; set; }
    [Required]
    public string Company { get; set; }
    [Required]
    public string JobTitle { get; set; }
    [Required]
    public string JobDescription { get; set; }
    [Required]
    public DateTime PostDate { get; set; }
    [Required]
    public DateTime ExpireDate { get; set; }

    public enum Commitment
    {
        PARTTIME,
        FULLTIME
    }

    [Required]
    public Commitment CommitmentType { get; set; }

    public enum WorkModel
    {
        REMOTE,
        HYBRID,
        ONSITE
    }

    [Required]
    public WorkModel WorkModelType { get; set; }

    [Required]
    public int MinSalary { get; set; }

    public ICollection<JobTag> JobTags { get; set; }
}