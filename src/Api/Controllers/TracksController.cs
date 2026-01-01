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
    public class TracksController(ITrackService trackService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TrackDto>>> GetAll()
        {
            var tracks = await trackService.GetAllAsync();
            return Ok(tracks);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TrackDto>> GetById(int id)
        {
            var track = await trackService.GetByIdAsync(id);
            if (track == null)
            {
                return NotFound();
            }
            return Ok(track);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TrackDto>> Create(TrackDto trackDto)
        {
            var createdTrack = await trackService.CreateAsync(trackDto);
            return CreatedAtAction(nameof(GetById), new { id = createdTrack.TrackId }, createdTrack);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<TrackDto>> Update(int id, TrackDto trackDto)
        {
            if (id != trackDto.TrackId)
            {
                return BadRequest();
            }
            var updatedTrack = await trackService.UpdateAsync(trackDto);
            return Ok(updatedTrack);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            await trackService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("paged")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<TrackDto>>> GetPaged([FromQuery] QueryParameters parameters)
        {
            var pagedTracks = await trackService.GetPagedAsync(parameters);
            return Ok(pagedTracks);
        }

    }
}

