namespace Application.Services.Implementation
{
    public class InvoiceServiceImplementation(
        IInvoiceRepository invoiceRepository,
        IMapper mapper,
        IValidator<InvoiceDto> validator)
        : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IValidator<InvoiceDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        //GetAll
        public async Task<IEnumerable<InvoiceDto>> GetAllAsync()
        {
            var invoices = await _invoiceRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        }

        //GetById
        public async Task<InvoiceDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var invoice = await _invoiceRepository.GetByIdAsync(id);
            return _mapper.Map<InvoiceDto?>(invoice);
        }

        //GetPaged
        public async Task<PaginatedResult<InvoiceDto>> GetPagedAsync(QueryParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var result = await _invoiceRepository.GetPagedAsync(
                parameters.PageNumber,
                parameters.PageSize,
                parameters.SearchTerm,
                parameters.SortColumn,
                parameters.IsDescending);

            return new PaginatedResult<InvoiceDto>
            {
                Items = _mapper.Map<IEnumerable<InvoiceDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        //Create
        public async Task<InvoiceDto> CreateAsync(InvoiceDto invoiceDto)
        {
            if (invoiceDto == null)
            {
                throw new ArgumentNullException(nameof(invoiceDto));
            }

            var validationResult = await _validator.ValidateAsync(invoiceDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var invoice = _mapper.Map<Invoice>(invoiceDto);
            var createdInvoice = await _invoiceRepository.AddAsync(invoice);
            return _mapper.Map<InvoiceDto>(createdInvoice);
        }

        //Update
        public async Task<InvoiceDto> UpdateAsync(InvoiceDto invoiceDto)
        {
            if (invoiceDto == null)
            {
                throw new ArgumentNullException(nameof(invoiceDto));
            }

            if (invoiceDto.InvoiceId <= 0)
            {
                throw new ArgumentException("Invoice Id must be greater than 0", nameof(invoiceDto.InvoiceId));
            }

            var validationResult = await _validator.ValidateAsync(invoiceDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var existingInvoice = await _invoiceRepository.GetByIdAsync(invoiceDto.InvoiceId);
            if (existingInvoice == null)
            {
                throw new KeyNotFoundException($"Invoice with ID {invoiceDto.InvoiceId} not found");
            }

            _mapper.Map(invoiceDto, existingInvoice);
            var updatedInvoice = await _invoiceRepository.UpdateAsync(existingInvoice);
            return _mapper.Map<InvoiceDto>(updatedInvoice);
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
            {
                throw new KeyNotFoundException($"Invoice with ID {id} not found");
            }

            await _invoiceRepository.DeleteAsync(id);
        }
    }
}

