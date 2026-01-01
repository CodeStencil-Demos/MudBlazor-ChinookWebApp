namespace Presentation.Services.HttpClients
{
    public class InvoiceHttpClient(HttpClient httpClient, IAuthService authService)
        : BaseHttpClient(httpClient, authService, "api/invoices"), IInvoiceService
    {
        public async Task<IEnumerable<InvoiceDto>> GetAllAsync() => 
            await base.GetAllAsync<InvoiceDto>();

        public async Task<InvoiceDto?> GetByIdAsync(int id) => 
            await base.GetByIdAsync<InvoiceDto>(id);

        public async Task<PaginatedResult<InvoiceDto>> GetPagedAsync(QueryParameters parameters) => 
            await base.GetPagedAsync<InvoiceDto>(parameters);

        public async Task<InvoiceDto> CreateAsync(InvoiceDto invoiceDto) => 
            await base.CreateAsync(invoiceDto);

        public async Task<InvoiceDto> UpdateAsync(InvoiceDto invoiceDto) => 
            await base.UpdateAsync(invoiceDto, invoiceDto.InvoiceId);

        public new async Task DeleteAsync(int id)
        {
            await base.DeleteAsync(id);
        }
    }
}
