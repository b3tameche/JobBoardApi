using Microsoft.EntityFrameworkCore;
using update.Database;

public class JobTagRepository : IJobTagRepository
{
    private readonly ApplicationDbContext _context;

    public JobTagRepository(ApplicationDbContext context) {
        this._context = context;
    }

    public async Task<string[]> GetTagsByJobId(int id) {
        string[] jobtags = await _context.JobTags
                                            .Where(each => each.JobId == id)
                                            .Select(each => each.Tag.TagName)
                                            .ToArrayAsync();
        return jobtags;
    }
}



