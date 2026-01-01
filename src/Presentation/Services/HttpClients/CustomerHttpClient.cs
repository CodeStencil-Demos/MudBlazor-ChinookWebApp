namespace Presentation.Services.HttpClients
{
    public class CustomerHttpClient(HttpClient httpClient, IAuthService authService)
        : BaseHttpClient(httpClient, authService, "api/customers"), ICustomerService
    {
        public async Task<IEnumerable<CustomerDto>> GetAllAsync() => 
            await base.GetAllAsync<CustomerDto>();

        public async Task<CustomerDto?> GetByIdAsync(int id) => 
            await base.GetByIdAsync<CustomerDto>(id);

        public async Task<PaginatedResult<CustomerDto>> GetPagedAsync(QueryParameters parameters) => 
            await base.GetPagedAsync<CustomerDto>(parameters);

        public async Task<CustomerDto> CreateAsync(CustomerDto customerDto) => 
            await base.CreateAsync(customerDto);

        public async Task<CustomerDto> UpdateAsync(CustomerDto customerDto) => 
            await base.UpdateAsync(customerDto, customerDto.CustomerId);

        public new async Task DeleteAsync(int id)
        {
            await base.DeleteAsync(id);
        }
    }
}
