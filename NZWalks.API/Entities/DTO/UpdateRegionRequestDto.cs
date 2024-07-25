using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Entities.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Code has to be atleast 1 character")]
        [MaxLength(3, ErrorMessage = "Code has to be less than 3 characters")]
        public string Code { get; set; } = string.Empty;
        [Required]
        [MaxLength(100, ErrorMessage = "Code has to be less than 100 characters")]
        public string Name { get; set; } = string.Empty;
        public string? RegionImageUrl { get; set; }
    }
}
