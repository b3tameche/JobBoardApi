



using update.Models.Domain;

public interface IJobTagRepository
{
    Task<string[]> GetTagsByJobId(int id);
}

