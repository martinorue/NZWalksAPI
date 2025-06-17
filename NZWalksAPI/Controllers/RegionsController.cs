using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        // constructor to inject the database context
        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var regionsDomain = dbContext.Regions.ToList();

            //Map Domain Model to DTO
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }

            if (regionsDto == null)
            {
                return NotFound();
            }
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            //Get Data from the database - Domain Models
            //var region = dbContext.Regions.Find(id);
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map/Convert Region Domain Model to Region DTO
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            //Return DTO back to the client
            return Ok(regionDto);
        }

        // POST: api/regions
        [HttpPost]
        public IActionResult Add([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Validate the request
            if (addRegionRequestDto == null || string.IsNullOrWhiteSpace(addRegionRequestDto.Code) || string.IsNullOrWhiteSpace(addRegionRequestDto.Name))
            {
                return BadRequest("Invalid region data.");
            }
            // Map DTO to Domain Model
            var regionDomain = new Region
            {
                Id = Guid.NewGuid(), // Generate a new unique identifier
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };
            // Add the new region to the database
            dbContext.Regions.Add(regionDomain);
            dbContext.SaveChanges();
            // Map Domain Model back to DTO for response
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        // PUT: api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Validate the request
            if (updateRegionRequestDto == null || string.IsNullOrWhiteSpace(updateRegionRequestDto.Code) || string.IsNullOrWhiteSpace(updateRegionRequestDto.Name))
            {
                return BadRequest("Invalid region data.");
            }
            // Find the existing region in the database
            var existingRegion = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (existingRegion == null)
            {
                return NotFound();
            }
            // Update the existing region's properties
            existingRegion.Code = updateRegionRequestDto.Code;
            existingRegion.Name = updateRegionRequestDto.Name;
            existingRegion.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
            // Save changes to the database
            dbContext.SaveChanges();
            // Map Domain Model back to DTO for response
            var regionDto = new RegionDto
            {
                Id = existingRegion.Id,
                Code = existingRegion.Code,
                Name = existingRegion.Name,
                RegionImageUrl = existingRegion.RegionImageUrl
            };
            return Ok(regionDto);
        }
    }
}