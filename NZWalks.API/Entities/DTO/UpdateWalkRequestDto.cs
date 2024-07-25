namespace NZWalks.API.Entities.DTO
{
    public class UpdateWalkRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        // Foreign Keys
        public Guid RegionId { get; set; }
        public Guid DifficultyId { get; set; }
    }
}
