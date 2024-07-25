namespace NZWalks.API.Entities.DTO
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        // Get the details 
        public RegionDto Region { get; set; } = new RegionDto();
        public DifficultyDto Difficulty { get; set; } = new DifficultyDto();
    }
}
