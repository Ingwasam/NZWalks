using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Entities;
using NZWalks.API.Entities.DTO;

namespace NZWalks.API.Repository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly DataContext _dataContext;
        public RegionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _dataContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetAsync(Guid id)
        {
            return await _dataContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Region> CreateAsync(Region region)
        {
            await _dataContext.Regions.AddAsync(region);
            await _dataContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var regionDomain = await _dataContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionDomain == null)
            {
                return null;
            }
            // Map to Domain Model
            regionDomain.Code = region.Code;
            regionDomain.Name = region.Name;
            regionDomain.RegionImageUrl = region.RegionImageUrl;


            await _dataContext.SaveChangesAsync();
            return regionDomain;
        }
        public async Task<Region?> DeleteAsync(Guid id)
        {
            var regionDomain = await _dataContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionDomain == null)
            {
                return null;
            }
            _dataContext.Regions.Remove(regionDomain);
            await _dataContext.SaveChangesAsync();
            return regionDomain;
        }
    }
}
