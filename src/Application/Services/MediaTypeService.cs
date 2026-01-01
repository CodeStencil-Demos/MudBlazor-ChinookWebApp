namespace Application.Services
{
    public class MediaTypeService(HttpClient httpClient) : BaseHttpService(httpClient, ApiBaseUrl), IMediaTypeService
    {
        private const string ApiBaseUrl = "api/MediaTypes";

        public Task<IEnumerable<MediaTypeDto>> GetAllAsync()
            => base.GetAllAsync<MediaTypeDto>();

        public Task<MediaTypeDto?> GetByIdAsync(int id)
            => base.GetByIdAsync<MediaTypeDto>(id);

        public Task<PaginatedResult<MediaTypeDto>> GetPagedAsync(QueryParameters parameters)
            => base.GetPagedAsync<MediaTypeDto>(parameters);

        public Task<MediaTypeDto> CreateAsync(MediaTypeDto mediatypeDto)
            => base.CreateAsync(mediatypeDto);

        public Task<MediaTypeDto> UpdateAsync(MediaTypeDto mediatypeDto)
            => base.UpdateAsync(mediatypeDto.MediaTypeId, mediatypeDto);

        public Task DeleteAsync(int id)
            => base.DeleteAsync(id);
    }
}
