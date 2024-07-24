using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Entities.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? RegionImageUrl { get; set; }
    }
}
