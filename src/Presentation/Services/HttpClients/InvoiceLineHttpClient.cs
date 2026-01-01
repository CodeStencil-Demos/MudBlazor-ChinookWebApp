namespace Presentation.Services.HttpClients
{
    public class InvoiceLineHttpClient(HttpClient httpClient, IAuthService authService)
        : BaseHttpClient(httpClient, authService, "api/invoicelines"), IInvoiceLineService
    {
        public async Task<IEnumerable<InvoiceLineDto>> GetAllAsync() => 
            await base.GetAllAsync<InvoiceLineDto>();

        public async Task<InvoiceLineDto?> GetByIdAsync(int id) => 
            await base.GetByIdAsync<InvoiceLineDto>(id);

        public async Task<PaginatedResult<InvoiceLineDto>> GetPagedAsync(QueryParameters parameters) => 
            await base.GetPagedAsync<InvoiceLineDto>(parameters);

        public async Task<InvoiceLineDto> CreateAsync(InvoiceLineDto invoicelineDto) => 
            await base.CreateAsync(invoicelineDto);

        public async Task<InvoiceLineDto> UpdateAsync(InvoiceLineDto invoicelineDto) => 
            await base.UpdateAsync(invoicelineDto, invoicelineDto.InvoiceLineId);

        public new async Task DeleteAsync(int id)
        {
            await base.DeleteAsync(id);
        }
    }
}
