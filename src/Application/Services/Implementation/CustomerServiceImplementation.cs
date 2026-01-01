namespace Application.Services.Implementation
{
    public class CustomerServiceImplementation(
        ICustomerRepository customerRepository,
        IMapper mapper,
        IValidator<CustomerDto> validator)
        : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<CustomerDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        //GetAll
        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        //GetById
        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var customer = await _customerRepository.GetByIdAsync(id);
            return _mapper.Map<CustomerDto?>(customer);
        }

        //GetPaged
        public async Task<PaginatedResult<CustomerDto>> GetPagedAsync(QueryParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var result = await _customerRepository.GetPagedAsync(
                parameters.PageNumber,
                parameters.PageSize,
                parameters.SearchTerm,
                parameters.SortColumn,
                parameters.IsDescending);

            return new PaginatedResult<CustomerDto>
            {
                Items = _mapper.Map<IEnumerable<CustomerDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        //Create
        public async Task<CustomerDto> CreateAsync(CustomerDto customerDto)
        {
            if (customerDto == null)
            {
                throw new ArgumentNullException(nameof(customerDto));
            }

            var validationResult = await _validator.ValidateAsync(customerDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var customer = _mapper.Map<Customer>(customerDto);
            var createdCustomer = await _customerRepository.AddAsync(customer);
            return _mapper.Map<CustomerDto>(createdCustomer);
        }

        //Update
        public async Task<CustomerDto> UpdateAsync(CustomerDto customerDto)
        {
            if (customerDto == null)
            {
                throw new ArgumentNullException(nameof(customerDto));
            }

            if (customerDto.CustomerId <= 0)
            {
                throw new ArgumentException("Customer Id must be greater than 0", nameof(customerDto.CustomerId));
            }

            var validationResult = await _validator.ValidateAsync(customerDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var existingCustomer = await _customerRepository.GetByIdAsync(customerDto.CustomerId);
            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customerDto.CustomerId} not found");
            }

            _mapper.Map(customerDto, existingCustomer);
            var updatedCustomer = await _customerRepository.UpdateAsync(existingCustomer);
            return _mapper.Map<CustomerDto>(updatedCustomer);
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found");
            }

            await _customerRepository.DeleteAsync(id);
        }
    }
}

