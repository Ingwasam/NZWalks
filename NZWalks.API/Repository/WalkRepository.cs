using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Entities;

namespace NZWalks.API.Repository
{
    public class WalkRepository : IWalkRepository
    {
        private readonly DataContext _dataContext;
        public WalkRepository(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _dataContext.Walks.AddAsync(walk);
            await _dataContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            List<Walk> walksDomain = await _dataContext.Walks.Include("Difficulty")
                .Include("Region").ToListAsync();
            return walksDomain;
        }

        public async Task<Walk?> GetAsync(Guid id)
        {
            return await _dataContext.Walks.Include("Difficulty")
                .Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var walkDomain = await _dataContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walkDomain == null)
            {
                return null;
            }
            walkDomain.Name = walk.Name;
            walkDomain.Description = walk.Description;
            walkDomain.LengthInKm = walk.LengthInKm;
            walkDomain.WalkImageUrl = walk.WalkImageUrl;
            walkDomain.DifficultyId = walk.DifficultyId;
            walkDomain.RegionId = walk.RegionId;

            await _dataContext.SaveChangesAsync();

            return walkDomain;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walkDomain = await _dataContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walkDomain == null)
            {
                return null;
            }
            _dataContext.Walks.Remove(walkDomain);
            await _dataContext.SaveChangesAsync();
            return walkDomain;
        }
    }
}
