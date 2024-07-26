using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Entities;
using NZWalks.API.Helpers;

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

        public async Task<List<Walk>> GetAllAsync(WalkQueryObject query)
        {
            var walksDomain = _dataContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering
            if (!string.IsNullOrWhiteSpace(query.FilterOn) && !string.IsNullOrWhiteSpace(query.FilterQuery))
            {
                if (query.FilterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walksDomain = walksDomain.Where(x => x.Name.Contains(query.FilterQuery));
                }
                else if (query.FilterOn.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    // Handle conditions for Length filtering
                    char[] comparisonOperators = new[] { '>', '<', '=' };
                    string operatorString = new string(query.FilterQuery.TakeWhile(c => comparisonOperators.Contains(c)).ToArray());
                    string lengthString = query.FilterQuery.Substring(operatorString.Length).Trim();

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
            if (string.IsNullOrWhiteSpace(query.SoryBy) == false)
            {
                if (query.SoryBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walksDomain = query.IsAscending ? walksDomain.OrderBy(x => x.Name) : walksDomain.OrderByDescending(x => x.Name);
                }
                else if (query.SoryBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walksDomain = query.IsAscending ? walksDomain.OrderBy(x => x.LengthInKm) : walksDomain.OrderByDescending(x => x.LengthInKm);
                }
            }

            // Pagination
            var skipResults = (query.PageNumber - 1) * query.PageSize;

            return await walksDomain.Skip(skipResults).Take(query.PageSize).ToListAsync();
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
