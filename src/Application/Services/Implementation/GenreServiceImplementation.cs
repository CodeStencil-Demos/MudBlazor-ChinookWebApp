namespace Application.Services.Implementation
{
    public class GenreServiceImplementation(
        IGenreRepository genreRepository,
        IMapper mapper,
        IValidator<GenreDto> validator)
        : IGenreService
    {
        private readonly IGenreRepository _genreRepository = genreRepository ?? throw new ArgumentNullException(nameof(genreRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<GenreDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        //GetAll
        public async Task<IEnumerable<GenreDto>> GetAllAsync()
        {
            var genres = await _genreRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GenreDto>>(genres);
        }

        //GetById
        public async Task<GenreDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var genre = await _genreRepository.GetByIdAsync(id);
            return _mapper.Map<GenreDto?>(genre);
        }

        //GetPaged
        public async Task<PaginatedResult<GenreDto>> GetPagedAsync(QueryParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var result = await _genreRepository.GetPagedAsync(
                parameters.PageNumber,
                parameters.PageSize,
                parameters.SearchTerm,
                parameters.SortColumn,
                parameters.IsDescending);

            return new PaginatedResult<GenreDto>
            {
                Items = _mapper.Map<IEnumerable<GenreDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        //Create
        public async Task<GenreDto> CreateAsync(GenreDto genreDto)
        {
            if (genreDto == null)
            {
                throw new ArgumentNullException(nameof(genreDto));
            }

            var validationResult = await _validator.ValidateAsync(genreDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var genre = _mapper.Map<Genre>(genreDto);
            var createdGenre = await _genreRepository.AddAsync(genre);
            return _mapper.Map<GenreDto>(createdGenre);
        }

        //Update
        public async Task<GenreDto> UpdateAsync(GenreDto genreDto)
        {
            if (genreDto == null)
            {
                throw new ArgumentNullException(nameof(genreDto));
            }

            if (genreDto.GenreId <= 0)
            {
                throw new ArgumentException("Genre Id must be greater than 0", nameof(genreDto.GenreId));
            }

            var validationResult = await _validator.ValidateAsync(genreDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var existingGenre = await _genreRepository.GetByIdAsync(genreDto.GenreId);
            if (existingGenre == null)
            {
                throw new KeyNotFoundException($"Genre with ID {genreDto.GenreId} not found");
            }

            _mapper.Map(genreDto, existingGenre);
            var updatedGenre = await _genreRepository.UpdateAsync(existingGenre);
            return _mapper.Map<GenreDto>(updatedGenre);
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null)
            {
                throw new KeyNotFoundException($"Genre with ID {id} not found");
            }

            await _genreRepository.DeleteAsync(id);
        }
    }
}

