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
    public class PlaylistsController(IPlaylistService playlistService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetAll()
        {
            var playlists = await playlistService.GetAllAsync();
            return Ok(playlists);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<PlaylistDto>> GetById(int id)
        {
            var playlist = await playlistService.GetByIdAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }
            return Ok(playlist);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PlaylistDto>> Create(PlaylistDto playlistDto)
        {
            var createdPlaylist = await playlistService.CreateAsync(playlistDto);
            return CreatedAtAction(nameof(GetById), new { id = createdPlaylist.PlaylistId }, createdPlaylist);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<PlaylistDto>> Update(int id, PlaylistDto playlistDto)
        {
            if (id != playlistDto.PlaylistId)
            {
                return BadRequest();
            }
            var updatedPlaylist = await playlistService.UpdateAsync(playlistDto);
            return Ok(updatedPlaylist);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            await playlistService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("paged")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<PlaylistDto>>> GetPaged([FromQuery] QueryParameters parameters)
        {
            var pagedPlaylists = await playlistService.GetPagedAsync(parameters);
            return Ok(pagedPlaylists);
        }

    }
}

