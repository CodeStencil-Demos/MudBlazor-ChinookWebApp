namespace Application.Services.Implementation
{
    public class EmployeeServiceImplementation(
        IEmployeeRepository employeeRepository,
        IMapper mapper,
        IValidator<EmployeeDto> validator)
        : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<EmployeeDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        //GetAll
        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        //GetById
        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var employee = await _employeeRepository.GetByIdAsync(id);
            return _mapper.Map<EmployeeDto?>(employee);
        }

        //GetPaged
        public async Task<PaginatedResult<EmployeeDto>> GetPagedAsync(QueryParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var result = await _employeeRepository.GetPagedAsync(
                parameters.PageNumber,
                parameters.PageSize,
                parameters.SearchTerm,
                parameters.SortColumn,
                parameters.IsDescending);

            return new PaginatedResult<EmployeeDto>
            {
                Items = _mapper.Map<IEnumerable<EmployeeDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        //Create
        public async Task<EmployeeDto> CreateAsync(EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                throw new ArgumentNullException(nameof(employeeDto));
            }

            var validationResult = await _validator.ValidateAsync(employeeDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var employee = _mapper.Map<Employee>(employeeDto);
            var createdEmployee = await _employeeRepository.AddAsync(employee);
            return _mapper.Map<EmployeeDto>(createdEmployee);
        }

        //Update
        public async Task<EmployeeDto> UpdateAsync(EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                throw new ArgumentNullException(nameof(employeeDto));
            }

            if (employeeDto.EmployeeId <= 0)
            {
                throw new ArgumentException("Employee Id must be greater than 0", nameof(employeeDto.EmployeeId));
            }

            var validationResult = await _validator.ValidateAsync(employeeDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var existingEmployee = await _employeeRepository.GetByIdAsync(employeeDto.EmployeeId);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {employeeDto.EmployeeId} not found");
            }

            _mapper.Map(employeeDto, existingEmployee);
            var updatedEmployee = await _employeeRepository.UpdateAsync(existingEmployee);
            return _mapper.Map<EmployeeDto>(updatedEmployee);
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found");
            }

            await _employeeRepository.DeleteAsync(id);
        }
    }
}

