namespace Application.Services.Implementation
{
    public class InvoiceLineServiceImplementation(
        IInvoiceLineRepository invoicelineRepository,
        IMapper mapper,
        IValidator<InvoiceLineDto> validator)
        : IInvoiceLineService
    {
        private readonly IInvoiceLineRepository _invoicelineRepository = invoicelineRepository ?? throw new ArgumentNullException(nameof(invoicelineRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<InvoiceLineDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        //GetAll
        public async Task<IEnumerable<InvoiceLineDto>> GetAllAsync()
        {
            var invoicelines = await _invoicelineRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<InvoiceLineDto>>(invoicelines);
        }

        //GetById
        public async Task<InvoiceLineDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var invoiceline = await _invoicelineRepository.GetByIdAsync(id);
            return _mapper.Map<InvoiceLineDto?>(invoiceline);
        }

        //GetPaged
        public async Task<PaginatedResult<InvoiceLineDto>> GetPagedAsync(QueryParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var result = await _invoicelineRepository.GetPagedAsync(
                parameters.PageNumber,
                parameters.PageSize,
                parameters.SearchTerm,
                parameters.SortColumn,
                parameters.IsDescending);

            return new PaginatedResult<InvoiceLineDto>
            {
                Items = _mapper.Map<IEnumerable<InvoiceLineDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        // Search

        //public async Task<IEnumerable<InvoiceLineDto>> SearchAsync(string searchTerm)
        //{
        //    if (string.IsNullOrWhiteSpace(searchTerm))
        //    {
        //        throw new ArgumentException("Search term cannot be null or empty", nameof(searchTerm));
        //    }

        //    var invoicelines = await _invoicelineRepository.SearchAsync(searchTerm);
        //    return _mapper.Map<IEnumerable<InvoiceLineDto>>(invoicelines);
        //}

        //Create
        public async Task<InvoiceLineDto> CreateAsync(InvoiceLineDto invoicelineDto)
        {
            if (invoicelineDto == null)
            {
                throw new ArgumentNullException(nameof(invoicelineDto));
            }

            var validationResult = await _validator.ValidateAsync(invoicelineDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var invoiceline = _mapper.Map<InvoiceLine>(invoicelineDto);
            var createdInvoiceLine = await _invoicelineRepository.AddAsync(invoiceline);
            return _mapper.Map<InvoiceLineDto>(createdInvoiceLine);
        }

        //Update
        public async Task<InvoiceLineDto> UpdateAsync(InvoiceLineDto invoicelineDto)
        {
            if (invoicelineDto == null)
            {
                throw new ArgumentNullException(nameof(invoicelineDto));
            }

            if (invoicelineDto.InvoiceLineId <= 0)
            {
                throw new ArgumentException("InvoiceLine Id must be greater than 0", nameof(invoicelineDto.InvoiceLineId));
            }

            var validationResult = await _validator.ValidateAsync(invoicelineDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var existingInvoiceLine = await _invoicelineRepository.GetByIdAsync(invoicelineDto.InvoiceLineId);
            if (existingInvoiceLine == null)
            {
                throw new KeyNotFoundException($"InvoiceLine with ID {invoicelineDto.InvoiceLineId} not found");
            }

            _mapper.Map(invoicelineDto, existingInvoiceLine);
            var updatedInvoiceLine = await _invoicelineRepository.UpdateAsync(existingInvoiceLine);
            return _mapper.Map<InvoiceLineDto>(updatedInvoiceLine);
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var invoiceline = await _invoicelineRepository.GetByIdAsync(id);
            if (invoiceline == null)
            {
                throw new KeyNotFoundException($"InvoiceLine with ID {id} not found");
            }

            await _invoicelineRepository.DeleteAsync(id);
        }
    }
}

