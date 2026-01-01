namespace Presentation.Services.HttpClients
{
    public class MediaTypeHttpClient(HttpClient httpClient, IAuthService authService)
        : BaseHttpClient(httpClient, authService, "api/mediatypes"), IMediaTypeService
    {
        public async Task<IEnumerable<MediaTypeDto>> GetAllAsync() => 
            await base.GetAllAsync<MediaTypeDto>();

        public async Task<MediaTypeDto?> GetByIdAsync(int id) => 
            await base.GetByIdAsync<MediaTypeDto>(id);

        public async Task<PaginatedResult<MediaTypeDto>> GetPagedAsync(QueryParameters parameters) => 
            await base.GetPagedAsync<MediaTypeDto>(parameters);

        public async Task<MediaTypeDto> CreateAsync(MediaTypeDto mediatypeDto) => 
            await base.CreateAsync(mediatypeDto);

        public async Task<MediaTypeDto> UpdateAsync(MediaTypeDto mediatypeDto) => 
            await base.UpdateAsync(mediatypeDto, mediatypeDto.MediaTypeId);

        public new async Task DeleteAsync(int id)
        {
            await base.DeleteAsync(id);
        }
    }
}
