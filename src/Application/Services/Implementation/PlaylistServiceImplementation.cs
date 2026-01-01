namespace Application.Services.Implementation
{
    public class PlaylistServiceImplementation(
        IPlaylistRepository playlistRepository,
        IMapper mapper,
        IValidator<PlaylistDto> validator)
        : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository = playlistRepository ?? throw new ArgumentNullException(nameof(playlistRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<PlaylistDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        //GetAll
        public async Task<IEnumerable<PlaylistDto>> GetAllAsync()
        {
            var playlists = await _playlistRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PlaylistDto>>(playlists);
        }

        //GetById
        public async Task<PlaylistDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var playlist = await _playlistRepository.GetByIdAsync(id);
            return _mapper.Map<PlaylistDto?>(playlist);
        }

        //GetPaged
        public async Task<PaginatedResult<PlaylistDto>> GetPagedAsync(QueryParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var result = await _playlistRepository.GetPagedAsync(
                parameters.PageNumber,
                parameters.PageSize,
                parameters.SearchTerm,
                parameters.SortColumn,
                parameters.IsDescending);

            return new PaginatedResult<PlaylistDto>
            {
                Items = _mapper.Map<IEnumerable<PlaylistDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        //Create
        public async Task<PlaylistDto> CreateAsync(PlaylistDto playlistDto)
        {
            if (playlistDto == null)
            {
                throw new ArgumentNullException(nameof(playlistDto));
            }

            var validationResult = await _validator.ValidateAsync(playlistDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var playlist = _mapper.Map<Playlist>(playlistDto);
            var createdPlaylist = await _playlistRepository.AddAsync(playlist);
            return _mapper.Map<PlaylistDto>(createdPlaylist);
        }

        //Update
        public async Task<PlaylistDto> UpdateAsync(PlaylistDto playlistDto)
        {
            if (playlistDto == null)
            {
                throw new ArgumentNullException(nameof(playlistDto));
            }

            if (playlistDto.PlaylistId <= 0)
            {
                throw new ArgumentException("Playlist Id must be greater than 0", nameof(playlistDto.PlaylistId));
            }

            var validationResult = await _validator.ValidateAsync(playlistDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var existingPlaylist = await _playlistRepository.GetByIdAsync(playlistDto.PlaylistId);
            if (existingPlaylist == null)
            {
                throw new KeyNotFoundException($"Playlist with ID {playlistDto.PlaylistId} not found");
            }

            _mapper.Map(playlistDto, existingPlaylist);
            var updatedPlaylist = await _playlistRepository.UpdateAsync(existingPlaylist);
            return _mapper.Map<PlaylistDto>(updatedPlaylist);
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var playlist = await _playlistRepository.GetByIdAsync(id);
            if (playlist == null)
            {
                throw new KeyNotFoundException($"Playlist with ID {id} not found");
            }

            await _playlistRepository.DeleteAsync(id);
        }
    }
}

