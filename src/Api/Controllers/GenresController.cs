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
    public class GenresController(IGenreService genreService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GenreDto>>> GetAll()
        {
            var genres = await genreService.GetAllAsync();
            return Ok(genres);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GenreDto>> GetById(int id)
        {
            var genre = await genreService.GetByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GenreDto>> Create(GenreDto genreDto)
        {
            var createdGenre = await genreService.CreateAsync(genreDto);
            return CreatedAtAction(nameof(GetById), new { id = createdGenre.GenreId }, createdGenre);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<GenreDto>> Update(int id, GenreDto genreDto)
        {
            if (id != genreDto.GenreId)
            {
                return BadRequest();
            }
            var updatedGenre = await genreService.UpdateAsync(genreDto);
            return Ok(updatedGenre);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            await genreService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("paged")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<GenreDto>>> GetPaged([FromQuery] QueryParameters parameters)
        {
            var pagedGenres = await genreService.GetPagedAsync(parameters);
            return Ok(pagedGenres);
        }

    }
}

