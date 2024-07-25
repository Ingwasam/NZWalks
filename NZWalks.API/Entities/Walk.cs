﻿namespace NZWalks.API.Entities
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        // Foreign Keys
        public Guid RegionId { get; set; }
        public Guid DifficultyId { get; set; }

        // Navigation Properties
        public Region Region { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}
