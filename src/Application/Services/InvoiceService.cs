namespace Application.Services
{
    public class InvoiceService(HttpClient httpClient) : BaseHttpService(httpClient, ApiBaseUrl), IInvoiceService
    {
        private const string ApiBaseUrl = "api/Invoices";

        public Task<IEnumerable<InvoiceDto>> GetAllAsync()
            => base.GetAllAsync<InvoiceDto>();

        public Task<InvoiceDto?> GetByIdAsync(int id)
            => base.GetByIdAsync<InvoiceDto>(id);

        public Task<PaginatedResult<InvoiceDto>> GetPagedAsync(QueryParameters parameters)
            => base.GetPagedAsync<InvoiceDto>(parameters);

        public Task<InvoiceDto> CreateAsync(InvoiceDto invoiceDto)
            => base.CreateAsync(invoiceDto);

        public Task<InvoiceDto> UpdateAsync(InvoiceDto invoiceDto)
            => base.UpdateAsync(invoiceDto.InvoiceId, invoiceDto);

        public Task DeleteAsync(int id)
            => base.DeleteAsync(id);
    }
}
