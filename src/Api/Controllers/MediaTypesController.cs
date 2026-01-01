using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Application.Common.Models;
using Application.DTOs;
using Application.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaTypesController(IMediaTypeService mediatypeService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MediaTypeDto>>> GetAll()
        {
            var mediatypes = await mediatypeService.GetAllAsync();
            return Ok(mediatypes);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<MediaTypeDto>> GetById(int id)
        {
            var mediatype = await mediatypeService.GetByIdAsync(id);
            if (mediatype == null)
            {
                return NotFound();
            }
            return Ok(mediatype);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MediaTypeDto>> Create(MediaTypeDto mediatypeDto)
        {
            var createdMediaType = await mediatypeService.CreateAsync(mediatypeDto);
            return CreatedAtAction(nameof(GetById), new { id = createdMediaType.MediaTypeId }, createdMediaType);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<MediaTypeDto>> Update(int id, MediaTypeDto mediatypeDto)
        {
            if (id != mediatypeDto.MediaTypeId)
            {
                return BadRequest();
            }
            var updatedMediaType = await mediatypeService.UpdateAsync(mediatypeDto);
            return Ok(updatedMediaType);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            await mediatypeService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("paged")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<MediaTypeDto>>> GetPaged([FromQuery] QueryParameters parameters)
        {
            var pagedMediaTypes = await mediatypeService.GetPagedAsync(parameters);
            return Ok(pagedMediaTypes);
        }

    }
}

