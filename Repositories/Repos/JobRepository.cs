using Microsoft.EntityFrameworkCore;
using update.Database;
using update.Models.Domain;
using update.Repositories.Interfaces;

namespace update.Repositories.Repos;

public class JobRepository : IJobRepository
{
    private readonly ApplicationDbContext _context;

    public JobRepository(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<Job> CreateAsync(Job job)
    {
        await _context.Jobs.AddAsync(job);
        await _context.SaveChangesAsync();
        return job;
    }

    public async Task<Job?> DeleteAsync(int id)
    {
        Job? found = await _context.Jobs.FirstOrDefaultAsync(job => job.Id == id);

        if (found == null) {
            return null;
        }

        _context.Jobs.Remove(found);
        await _context.SaveChangesAsync();

        return found;
    }

    public async Task<List<Job>> GetAllAsync() {
        return await _context.Jobs.ToListAsync();
    }

    public async Task<List<Job>> GetAllByUserId(string id)
    {
        return await _context.Jobs.Where(each => each.User.Id == id).ToListAsync();
    }

    public async Task<Job?> GetByIdAsync(int id)
    {
        return await _context.Jobs.FirstOrDefaultAsync(each => each.Id == id);
    }

    public async Task<Job?> UpdateAsync(int id, Job job)
    {
        Job? found = await _context.Jobs.FirstOrDefaultAsync(job => job.Id == id);

        if (found == null) {
            return null;
        }

        found.Id = job.Id;
        found.JobTitle = job.JobDescription;
        found.PostDate = job.PostDate;
        found.ExpireDate = job.ExpireDate;
        found.CommitmentType = job.CommitmentType;
        found.WorkModelType = job.WorkModelType;

        await _context.SaveChangesAsync();
        return found;
    }
}