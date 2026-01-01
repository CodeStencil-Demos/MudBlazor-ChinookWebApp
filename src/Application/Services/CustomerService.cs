namespace Application.Services
{
    public class CustomerService(HttpClient httpClient) : BaseHttpService(httpClient, ApiBaseUrl), ICustomerService
    {
        private const string ApiBaseUrl = "api/Customers";

        public Task<IEnumerable<CustomerDto>> GetAllAsync()
            => base.GetAllAsync<CustomerDto>();

        public Task<CustomerDto?> GetByIdAsync(int id)
            => base.GetByIdAsync<CustomerDto>(id);

        public Task<PaginatedResult<CustomerDto>> GetPagedAsync(QueryParameters parameters)
            => base.GetPagedAsync<CustomerDto>(parameters);

        public Task<CustomerDto> CreateAsync(CustomerDto customerDto)
            => base.CreateAsync(customerDto);

        public Task<CustomerDto> UpdateAsync(CustomerDto customerDto)
            => base.UpdateAsync(customerDto.CustomerId, customerDto);

        public Task DeleteAsync(int id)
            => base.DeleteAsync(id);
    }
}
