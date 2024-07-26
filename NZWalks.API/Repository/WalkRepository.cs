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

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 100)
        {
            var walksDomain = _dataContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walksDomain = walksDomain.Where(x => x.Name.Contains(filterQuery));
                }
                else if (filterOn.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    // Handle conditions for Length filtering
                    char[] comparisonOperators = new[] { '>', '<', '=' };
                    string operatorString = new string(filterQuery.TakeWhile(c => comparisonOperators.Contains(c)).ToArray());
                    string lengthString = filterQuery.Substring(operatorString.Length).Trim();

                    if (double.TryParse(lengthString, out double length))
                    {
                        if (operatorString == ">")
                        {
                            walksDomain = walksDomain.Where(x => x.LengthInKm > length);
                        }
                        else if (operatorString == ">=")
                        {
                            walksDomain = walksDomain.Where(x => x.LengthInKm >= length);
                        }
                        else if (operatorString == "<")
                        {
                            walksDomain = walksDomain.Where(x => x.LengthInKm < length);
                        }
                        else if (operatorString == "<=")
                        {
                            walksDomain = walksDomain.Where(x => x.LengthInKm <= length);
                        }
                        else if (operatorString == "=" || operatorString == "==")
                        {
                            walksDomain = walksDomain.Where(x => x.LengthInKm == length);
                        }
                        else
                        {
                            // Handle invalid operator by ignoring the filter
                        }
                    }
                }
            }

            // Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walksDomain = isAscending ? walksDomain.OrderBy(x => x.Name) : walksDomain.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walksDomain = isAscending ? walksDomain.OrderBy(x => x.LengthInKm) : walksDomain.OrderByDescending(x => x.LengthInKm);
                }
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await walksDomain.Skip(skipResults).Take(pageSize).ToListAsync();
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
