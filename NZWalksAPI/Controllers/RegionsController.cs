using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Respositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        // constructor to inject the database context
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get Data from the database - Domain Models
            var regionsDomain = await regionRepository.GetAllAsync();

            // Map Domian Model to DTO using AutoMapper
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);
            
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Get Data from the database - Domain Models
            //var region = dbContext.Regions.Find(id);
            //var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO using AutoMapper
            var regionDto = mapper.Map<RegionDto>(regionDomain);

            //Return DTO back to the client
            return Ok(regionDto);
        }

        // POST: api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Validate the request
            if (addRegionRequestDto == null || string.IsNullOrWhiteSpace(addRegionRequestDto.Code) || string.IsNullOrWhiteSpace(addRegionRequestDto.Name))
            {
                return BadRequest("Invalid region data.");
            }

            // Map DTO to Domain Model using AutoMapper
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
            // Add the new region to the database
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);


            // Map Domain Model to DTO using AutoMapper
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        // PUT: api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Validate the request
            if (updateRegionRequestDto == null || string.IsNullOrWhiteSpace(updateRegionRequestDto.Code) || string.IsNullOrWhiteSpace(updateRegionRequestDto.Name))
            {
                return BadRequest("Invalid region data.");
            }

            //Map DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound(); // If the region was not found
            }

            // Map DTO to Domain Model
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }

        // DELETE: api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // return deleted Region back
            // map Domain Model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }
    }
}