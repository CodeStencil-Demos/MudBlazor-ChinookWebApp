namespace Presentation.Services.HttpClients
{
    public class PlaylistHttpClient(HttpClient httpClient, IAuthService authService)
        : BaseHttpClient(httpClient, authService, "api/playlists"), IPlaylistService
    {
        public async Task<IEnumerable<PlaylistDto>> GetAllAsync() => 
            await base.GetAllAsync<PlaylistDto>();

        public async Task<PlaylistDto?> GetByIdAsync(int id) => 
            await base.GetByIdAsync<PlaylistDto>(id);

        public async Task<PaginatedResult<PlaylistDto>> GetPagedAsync(QueryParameters parameters) => 
            await base.GetPagedAsync<PlaylistDto>(parameters);

        public async Task<PlaylistDto> CreateAsync(PlaylistDto playlistDto) => 
            await base.CreateAsync(playlistDto);

        public async Task<PlaylistDto> UpdateAsync(PlaylistDto playlistDto) => 
            await base.UpdateAsync(playlistDto, playlistDto.PlaylistId);

        public new async Task DeleteAsync(int id)
        {
            await base.DeleteAsync(id);
        }
    }
}
