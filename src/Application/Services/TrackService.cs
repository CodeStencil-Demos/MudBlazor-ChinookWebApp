namespace Application.Services
{
    public class TrackService(HttpClient httpClient) : BaseHttpService(httpClient, ApiBaseUrl), ITrackService
    {
        private const string ApiBaseUrl = "api/Tracks";

        public Task<IEnumerable<TrackDto>> GetAllAsync()
            => base.GetAllAsync<TrackDto>();

        public Task<TrackDto?> GetByIdAsync(int id)
            => base.GetByIdAsync<TrackDto>(id);

        public Task<PaginatedResult<TrackDto>> GetPagedAsync(QueryParameters parameters)
            => base.GetPagedAsync<TrackDto>(parameters);

        public Task<TrackDto> CreateAsync(TrackDto trackDto)
            => base.CreateAsync(trackDto);

        public Task<TrackDto> UpdateAsync(TrackDto trackDto)
            => base.UpdateAsync(trackDto.TrackId, trackDto);

        public Task DeleteAsync(int id)
            => base.DeleteAsync(id);
    }
}
