using System.ComponentModel.DataAnnotations;

namespace update.Models.Domain;

public class Tag
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string TagName { get; set; }

    public ICollection<JobTag> JobTags { get; set; }
}