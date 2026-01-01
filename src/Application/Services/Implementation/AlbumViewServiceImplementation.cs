namespace Application.Services.Implementation
{
    public class AlbumViewServiceImplementation(
        IAlbumViewRepository albumViewRepository,
        IMapper mapper)
        : IAlbumViewService
    {
        private readonly IAlbumViewRepository _albumViewRepository = albumViewRepository ?? throw new ArgumentNullException(nameof(albumViewRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        //GetAll
        public async Task<IEnumerable<AlbumViewDto>> GetAllAsync()
        {
            var albumViews = await _albumViewRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AlbumViewDto>>(albumViews);
        }

        //GetPaged
        public async Task<PaginatedResult<AlbumViewDto>> GetPagedAsync(QueryParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var result = await _albumViewRepository.GetPagedAsync(
                parameters.PageNumber,
                parameters.PageSize,
                parameters.SearchTerm,
                parameters.SortColumn,
                parameters.IsDescending);

            return new PaginatedResult<AlbumViewDto>
            {
                Items = _mapper.Map<IEnumerable<AlbumViewDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        //Search
        public async Task<IEnumerable<AlbumViewDto>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentException("Search term cannot be null or empty", nameof(searchTerm));
            }

            var albumViews = await _albumViewRepository.SearchAsync(searchTerm);
            return _mapper.Map<IEnumerable<AlbumViewDto>>(albumViews);
        }
    }
}
