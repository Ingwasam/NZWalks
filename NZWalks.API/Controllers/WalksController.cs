using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Entities;
using NZWalks.API.Entities.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            if (ModelState.IsValid)
            {
                var walkDomain = _mapper.Map<Walk>(addWalkRequestDto);
                walkDomain = await _walkRepository.CreateAsync(walkDomain);
                var walkDto = _mapper.Map<WalkDto>(walkDomain);
                return CreatedAtAction(nameof(Get), new {id = walkDto.Id}, walkDto);
            }
            return BadRequest(addWalkRequestDto);


        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walksDomain = await _walkRepository.GetAllAsync();
            var walksDto = _mapper.Map<List<WalkDto>>(walksDomain);
            return Ok(walksDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Get([FromRoute]Guid id)
        {
            var walkDomain = await _walkRepository.GetAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }
            var walkDto = _mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            if (ModelState.IsValid)
            {
                var walkDomain = _mapper.Map<Walk>(updateWalkRequestDto);
                walkDomain = await _walkRepository.UpdateAsync(id, walkDomain);
                if (walkDomain == null)
                {
                    return NotFound();
                }
                var walkDto = _mapper.Map<WalkDto>(walkDomain);
                return Ok(walkDto);
            }
            return BadRequest(ModelState);
            
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomain = await _walkRepository.DeleteAsync(id);
            if (deletedWalkDomain == null)
            {
                return NotFound();
            }
            var walkDto = _mapper.Map<WalkDto>(deletedWalkDomain);
            return Ok(walkDto);
        }
    }
}
