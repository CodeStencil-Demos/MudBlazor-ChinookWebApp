namespace Presentation.Services.HttpClients
{
    public class EmployeeHttpClient(HttpClient httpClient, IAuthService authService)
        : BaseHttpClient(httpClient, authService, "api/employees"), IEmployeeService
    {
        public async Task<IEnumerable<EmployeeDto>> GetAllAsync() => 
            await base.GetAllAsync<EmployeeDto>();

        public async Task<EmployeeDto?> GetByIdAsync(int id) => 
            await base.GetByIdAsync<EmployeeDto>(id);

        public async Task<PaginatedResult<EmployeeDto>> GetPagedAsync(QueryParameters parameters) => 
            await base.GetPagedAsync<EmployeeDto>(parameters);

        public async Task<EmployeeDto> CreateAsync(EmployeeDto employeeDto) => 
            await base.CreateAsync(employeeDto);

        public async Task<EmployeeDto> UpdateAsync(EmployeeDto employeeDto) => 
            await base.UpdateAsync(employeeDto, employeeDto.EmployeeId);

        public new async Task DeleteAsync(int id)
        {
            await base.DeleteAsync(id);
        }
    }
}
