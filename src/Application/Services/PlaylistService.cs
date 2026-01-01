namespace Application.Services
{
    public class PlaylistService(HttpClient httpClient) : BaseHttpService(httpClient, ApiBaseUrl), IPlaylistService
    {
        private const string ApiBaseUrl = "api/Playlists";

        public Task<IEnumerable<PlaylistDto>> GetAllAsync()
            => base.GetAllAsync<PlaylistDto>();

        public Task<PlaylistDto?> GetByIdAsync(int id)
            => base.GetByIdAsync<PlaylistDto>(id);

        public Task<PaginatedResult<PlaylistDto>> GetPagedAsync(QueryParameters parameters)
            => base.GetPagedAsync<PlaylistDto>(parameters);

        public Task<PlaylistDto> CreateAsync(PlaylistDto playlistDto)
            => base.CreateAsync(playlistDto);

        public Task<PlaylistDto> UpdateAsync(PlaylistDto playlistDto)
            => base.UpdateAsync(playlistDto.PlaylistId, playlistDto);

        public Task DeleteAsync(int id)
            => base.DeleteAsync(id);
    }
}
