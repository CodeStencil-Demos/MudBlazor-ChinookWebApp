namespace Presentation.Services.HttpClients
{
    public class TrackHttpClient(HttpClient httpClient, IAuthService authService)
        : BaseHttpClient(httpClient, authService, "api/tracks"), ITrackService
    {
        public async Task<IEnumerable<TrackDto>> GetAllAsync() => 
            await base.GetAllAsync<TrackDto>();

        public async Task<TrackDto?> GetByIdAsync(int id) => 
            await base.GetByIdAsync<TrackDto>(id);

        public async Task<PaginatedResult<TrackDto>> GetPagedAsync(QueryParameters parameters) => 
            await base.GetPagedAsync<TrackDto>(parameters);

        public async Task<TrackDto> CreateAsync(TrackDto trackDto) => 
            await base.CreateAsync(trackDto);

        public async Task<TrackDto> UpdateAsync(TrackDto trackDto) => 
            await base.UpdateAsync(trackDto, trackDto.TrackId);

        public new async Task DeleteAsync(int id)
        {
            await base.DeleteAsync(id);
        }
    }
}
