namespace Application.Services.Implementation
{
    public class ArtistServiceImplementation(
        IArtistRepository artistRepository,
        IMapper mapper,
        IValidator<ArtistDto> validator)
        : IArtistService
    {
        private readonly IArtistRepository _artistRepository = artistRepository ?? throw new ArgumentNullException(nameof(artistRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<ArtistDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        //GetAll
        public async Task<IEnumerable<ArtistDto>> GetAllAsync()
        {
            var artists = await _artistRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ArtistDto>>(artists);
        }

        //GetById
        public async Task<ArtistDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var artist = await _artistRepository.GetByIdAsync(id);
            return _mapper.Map<ArtistDto?>(artist);
        }

        //GetPaged
        public async Task<PaginatedResult<ArtistDto>> GetPagedAsync(QueryParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var result = await _artistRepository.GetPagedAsync(
                parameters.PageNumber,
                parameters.PageSize,
                parameters.SearchTerm,
                parameters.SortColumn,
                parameters.IsDescending);

            return new PaginatedResult<ArtistDto>
            {
                Items = _mapper.Map<IEnumerable<ArtistDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        //Create
        public async Task<ArtistDto> CreateAsync(ArtistDto artistDto)
        {
            if (artistDto == null)
            {
                throw new ArgumentNullException(nameof(artistDto));
            }

            var validationResult = await _validator.ValidateAsync(artistDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var artist = _mapper.Map<Artist>(artistDto);
            var createdArtist = await _artistRepository.AddAsync(artist);
            return _mapper.Map<ArtistDto>(createdArtist);
        }

        //Update
        public async Task<ArtistDto> UpdateAsync(ArtistDto artistDto)
        {
            if (artistDto == null)
            {
                throw new ArgumentNullException(nameof(artistDto));
            }

            if (artistDto.ArtistId <= 0)
            {
                throw new ArgumentException("Artist Id must be greater than 0", nameof(artistDto.ArtistId));
            }

            var validationResult = await _validator.ValidateAsync(artistDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var existingArtist = await _artistRepository.GetByIdAsync(artistDto.ArtistId);
            if (existingArtist == null)
            {
                throw new KeyNotFoundException($"Artist with ID {artistDto.ArtistId} not found");
            }

            _mapper.Map(artistDto, existingArtist);
            var updatedArtist = await _artistRepository.UpdateAsync(existingArtist);
            return _mapper.Map<ArtistDto>(updatedArtist);
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var artist = await _artistRepository.GetByIdAsync(id);
            if (artist == null)
            {
                throw new KeyNotFoundException($"Artist with ID {id} not found");
            }

            await _artistRepository.DeleteAsync(id);
        }
    }
}

