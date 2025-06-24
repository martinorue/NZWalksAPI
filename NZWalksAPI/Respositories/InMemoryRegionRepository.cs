using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Respositories
{
    public class InMemoryRegionRepository : IRegionRepository
    {
        public Task<Region> CreateAsync(Region region)
        {
            throw new NotImplementedException();
        }

        public Task<Region?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return new List<Region>
            { 
                new Region()
                {
                    Id = Guid.NewGuid(),
                    Code = "NI",
                    Name = "North Island",
                    RegionImageUrl = "https://example.com/north-island.jpg"
                },
            };
        }

        public Task<Region?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Region?> UpdateAsync(Guid id, Region region)
        {
            throw new NotImplementedException();
        }
    }
}
