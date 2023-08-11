using static update.Models.Domain.Job;

namespace update.Models.DTOs;

public class JobDTO
{   
    public int Id { get; set; }
    public string JobTitle { get; set; }
    public string JobDescription { get; set; }
    public string Company { get; set; }
    public Commitment CommitmentType { get; set; }
    public WorkModel WorkModelType { get; set; }
    public int MinSalary { get; set; }
    public string[] Tags { get; set; }
}