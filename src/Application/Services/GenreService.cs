namespace Application.Services
{
    public class GenreService(HttpClient httpClient) : BaseHttpService(httpClient, ApiBaseUrl), IGenreService
    {
        private const string ApiBaseUrl = "api/Genres";

        public Task<IEnumerable<GenreDto>> GetAllAsync()
            => base.GetAllAsync<GenreDto>();

        public Task<GenreDto?> GetByIdAsync(int id)
            => base.GetByIdAsync<GenreDto>(id);

        public Task<PaginatedResult<GenreDto>> GetPagedAsync(QueryParameters parameters)
            => base.GetPagedAsync<GenreDto>(parameters);

        public Task<GenreDto> CreateAsync(GenreDto genreDto)
            => base.CreateAsync(genreDto);

        public Task<GenreDto> UpdateAsync(GenreDto genreDto)
            => base.UpdateAsync(genreDto.GenreId, genreDto);

        public Task DeleteAsync(int id)
            => base.DeleteAsync(id);
    }
}
