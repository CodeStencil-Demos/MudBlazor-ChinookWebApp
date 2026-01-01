namespace Application.Services.Implementation
{
    public class MediaTypeServiceImplementation(
        IMediaTypeRepository mediatypeRepository,
        IMapper mapper,
        IValidator<MediaTypeDto> validator)
        : IMediaTypeService
    {
        private readonly IMediaTypeRepository _mediatypeRepository = mediatypeRepository ?? throw new ArgumentNullException(nameof(mediatypeRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<MediaTypeDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        //GetAll
        public async Task<IEnumerable<MediaTypeDto>> GetAllAsync()
        {
            var mediatypes = await _mediatypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MediaTypeDto>>(mediatypes);
        }

        //GetById
        public async Task<MediaTypeDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var mediatype = await _mediatypeRepository.GetByIdAsync(id);
            return _mapper.Map<MediaTypeDto?>(mediatype);
        }

        //GetPaged
        public async Task<PaginatedResult<MediaTypeDto>> GetPagedAsync(QueryParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var result = await _mediatypeRepository.GetPagedAsync(
                parameters.PageNumber,
                parameters.PageSize,
                parameters.SearchTerm,
                parameters.SortColumn,
                parameters.IsDescending);

            return new PaginatedResult<MediaTypeDto>
            {
                Items = _mapper.Map<IEnumerable<MediaTypeDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        //Create
        public async Task<MediaTypeDto> CreateAsync(MediaTypeDto mediatypeDto)
        {
            if (mediatypeDto == null)
            {
                throw new ArgumentNullException(nameof(mediatypeDto));
            }

            var validationResult = await _validator.ValidateAsync(mediatypeDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var mediatype = _mapper.Map<MediaType>(mediatypeDto);
            var createdMediaType = await _mediatypeRepository.AddAsync(mediatype);
            return _mapper.Map<MediaTypeDto>(createdMediaType);
        }

        //Update
        public async Task<MediaTypeDto> UpdateAsync(MediaTypeDto mediatypeDto)
        {
            if (mediatypeDto == null)
            {
                throw new ArgumentNullException(nameof(mediatypeDto));
            }

            if (mediatypeDto.MediaTypeId <= 0)
            {
                throw new ArgumentException("MediaType Id must be greater than 0", nameof(mediatypeDto.MediaTypeId));
            }

            var validationResult = await _validator.ValidateAsync(mediatypeDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var existingMediaType = await _mediatypeRepository.GetByIdAsync(mediatypeDto.MediaTypeId);
            if (existingMediaType == null)
            {
                throw new KeyNotFoundException($"MediaType with ID {mediatypeDto.MediaTypeId} not found");
            }

            _mapper.Map(mediatypeDto, existingMediaType);
            var updatedMediaType = await _mediatypeRepository.UpdateAsync(existingMediaType);
            return _mapper.Map<MediaTypeDto>(updatedMediaType);
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var mediatype = await _mediatypeRepository.GetByIdAsync(id);
            if (mediatype == null)
            {
                throw new KeyNotFoundException($"MediaType with ID {id} not found");
            }

            await _mediatypeRepository.DeleteAsync(id);
        }
    }
}

