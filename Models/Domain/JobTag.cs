using update.Models.Domain;

public class JobTag {
    public int JobId { get; set; }
    public Job Job { get; set; }
    public int TagId { get; set; }
    public Tag Tag { get; set; }
}