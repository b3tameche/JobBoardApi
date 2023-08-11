using update.Models.Domain;

namespace update.Repositories.Interfaces;

public interface ITagRepository
{
    Task<List<Tag>> GetAllAsync();
}