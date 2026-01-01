namespace Presentation.Services.HttpClients
{
    public class GenreHttpClient(HttpClient httpClient, IAuthService authService)
        : BaseHttpClient(httpClient, authService, "api/genres"), IGenreService
    {
        public async Task<IEnumerable<GenreDto>> GetAllAsync() => 
            await base.GetAllAsync<GenreDto>();

        public async Task<GenreDto?> GetByIdAsync(int id) => 
            await base.GetByIdAsync<GenreDto>(id);

        public async Task<PaginatedResult<GenreDto>> GetPagedAsync(QueryParameters parameters) => 
            await base.GetPagedAsync<GenreDto>(parameters);

        public async Task<GenreDto> CreateAsync(GenreDto genreDto) => 
            await base.CreateAsync(genreDto);

        public async Task<GenreDto> UpdateAsync(GenreDto genreDto) => 
            await base.UpdateAsync(genreDto, genreDto.GenreId);

        public new async Task DeleteAsync(int id)
        {
            await base.DeleteAsync(id);
        }
    }
}
