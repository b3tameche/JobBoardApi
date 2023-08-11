using Microsoft.EntityFrameworkCore;
using update.Database;
using update.Models.Domain;
using update.Repositories.Interfaces;

namespace update.Repositories.Repos;

public class TagRepository : ITagRepository
{
    private readonly ApplicationDbContext _context;

    public TagRepository(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<List<Tag>> GetAllAsync()
    {
        return await _context.Tags.ToListAsync();
    }
}