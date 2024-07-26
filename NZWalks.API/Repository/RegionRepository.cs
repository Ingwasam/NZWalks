using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Entities;
using NZWalks.API.Entities.DTO;
using NZWalks.API.Helpers;

namespace NZWalks.API.Repository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly DataContext _dataContext;
        public RegionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IEnumerable<Region>> GetAllAsync(RegionQueryObject query)
        {
            var regionsDomain = _dataContext.Regions.AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(query.FilterBy) == false && !string.IsNullOrWhiteSpace(query.FilterQuery))
            {
                if (query.FilterBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    regionsDomain = regionsDomain.Where(x => x.Name.Contains(query.FilterQuery));
                }
            }

            // Sorting
            if (string.IsNullOrWhiteSpace(query.SortBy) == false)
            {
                if (query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    regionsDomain = query.IsAscending ? regionsDomain.OrderBy(x => x.Name) : regionsDomain.OrderByDescending(x => x.Name);
                }
            }

            // Pagination
            var skipResults = (query.PageNumber - 1) * query.PageSize;
            return await regionsDomain.Skip(skipResults).Take(query.PageSize).ToListAsync();
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
