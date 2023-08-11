using System.Threading.Tasks.Dataflow;
using update.Models.Domain;
using update.Models.DTOs;

namespace update.Repositories.Interfaces;

public interface IJobRepository
{
    Task<List<Job>> GetAllAsync();

    Task<Job?> GetByIdAsync(int id);

    Task<Job> CreateAsync(Job job);

    Task<Job?> UpdateAsync(int id, Job job);

    Task<Job?> DeleteAsync(int id);

    Task<List<Job>> GetAllByUserId(string id);
}