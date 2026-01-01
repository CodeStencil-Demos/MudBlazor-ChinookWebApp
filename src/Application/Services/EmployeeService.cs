namespace Application.Services
{
    public class EmployeeService(HttpClient httpClient) : BaseHttpService(httpClient, ApiBaseUrl), IEmployeeService
    {
        private const string ApiBaseUrl = "api/Employees";

        public Task<IEnumerable<EmployeeDto>> GetAllAsync()
            => base.GetAllAsync<EmployeeDto>();

        public Task<EmployeeDto?> GetByIdAsync(int id)
            => base.GetByIdAsync<EmployeeDto>(id);

        public Task<PaginatedResult<EmployeeDto>> GetPagedAsync(QueryParameters parameters)
            => base.GetPagedAsync<EmployeeDto>(parameters);

        public Task<EmployeeDto> CreateAsync(EmployeeDto employeeDto)
            => base.CreateAsync(employeeDto);

        public Task<EmployeeDto> UpdateAsync(EmployeeDto employeeDto)
            => base.UpdateAsync(employeeDto.EmployeeId, employeeDto);

        public Task DeleteAsync(int id)
            => base.DeleteAsync(id);
    }
}
