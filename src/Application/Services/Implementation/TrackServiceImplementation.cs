namespace Application.Services.Implementation
{
    public class TrackServiceImplementation(
        ITrackRepository trackRepository,
        IMapper mapper,
        IValidator<TrackDto> validator)
        : ITrackService
    {
        private readonly ITrackRepository _trackRepository = trackRepository ?? throw new ArgumentNullException(nameof(trackRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<TrackDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        //GetAll
        public async Task<IEnumerable<TrackDto>> GetAllAsync()
        {
            var tracks = await _trackRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TrackDto>>(tracks);
        }

        //GetById
        public async Task<TrackDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var track = await _trackRepository.GetByIdAsync(id);
            return _mapper.Map<TrackDto?>(track);
        }

        //GetPaged
        public async Task<PaginatedResult<TrackDto>> GetPagedAsync(QueryParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var result = await _trackRepository.GetPagedAsync(
                parameters.PageNumber,
                parameters.PageSize,
                parameters.SearchTerm,
                parameters.SortColumn,
                parameters.IsDescending);

            return new PaginatedResult<TrackDto>
            {
                Items = _mapper.Map<IEnumerable<TrackDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        //Create
        public async Task<TrackDto> CreateAsync(TrackDto trackDto)
        {
            if (trackDto == null)
            {
                throw new ArgumentNullException(nameof(trackDto));
            }

            var validationResult = await _validator.ValidateAsync(trackDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var track = _mapper.Map<Track>(trackDto);
            var createdTrack = await _trackRepository.AddAsync(track);
            return _mapper.Map<TrackDto>(createdTrack);
        }

        //Update
        public async Task<TrackDto> UpdateAsync(TrackDto trackDto)
        {
            if (trackDto == null)
            {
                throw new ArgumentNullException(nameof(trackDto));
            }

            if (trackDto.TrackId <= 0)
            {
                throw new ArgumentException("Track Id must be greater than 0", nameof(trackDto.TrackId));
            }

            var validationResult = await _validator.ValidateAsync(trackDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var existingTrack = await _trackRepository.GetByIdAsync(trackDto.TrackId);
            if (existingTrack == null)
            {
                throw new KeyNotFoundException($"Track with ID {trackDto.TrackId} not found");
            }

            _mapper.Map(trackDto, existingTrack);
            var updatedTrack = await _trackRepository.UpdateAsync(existingTrack);
            return _mapper.Map<TrackDto>(updatedTrack);
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var track = await _trackRepository.GetByIdAsync(id);
            if (track == null)
            {
                throw new KeyNotFoundException($"Track with ID {id} not found");
            }

            await _trackRepository.DeleteAsync(id);
        }
    }
}

