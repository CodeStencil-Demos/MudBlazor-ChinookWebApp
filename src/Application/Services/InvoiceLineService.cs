namespace Application.Services
{
    public class InvoiceLineService(HttpClient httpClient) : BaseHttpService(httpClient, ApiBaseUrl), IInvoiceLineService
    {
        private const string ApiBaseUrl = "api/InvoiceLines";

        public Task<IEnumerable<InvoiceLineDto>> GetAllAsync()
            => base.GetAllAsync<InvoiceLineDto>();

        public Task<InvoiceLineDto?> GetByIdAsync(int id)
            => base.GetByIdAsync<InvoiceLineDto>(id);

        public Task<PaginatedResult<InvoiceLineDto>> GetPagedAsync(QueryParameters parameters)
            => base.GetPagedAsync<InvoiceLineDto>(parameters);

        public Task<InvoiceLineDto> CreateAsync(InvoiceLineDto invoicelineDto)
            => base.CreateAsync(invoicelineDto);

        public Task<InvoiceLineDto> UpdateAsync(InvoiceLineDto invoicelineDto)
            => base.UpdateAsync(invoicelineDto.InvoiceLineId, invoicelineDto);

        public Task DeleteAsync(int id)
            => base.DeleteAsync(id);
    }
}
