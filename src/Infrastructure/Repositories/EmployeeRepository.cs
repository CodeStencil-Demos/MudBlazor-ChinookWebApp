namespace Infrastructure.Repositories
{
    public class EmployeeRepository(ChinookWebAppContext context) : IEmployeeRepository
    {

        //GetAll
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await context.Employees
                .Include(a => a.ReportsToNavigation)
                .ToListAsync();
        }

        //GetById
        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await context.Employees
                .Include(a => a.ReportsToNavigation)
                .FirstOrDefaultAsync(a => a.EmployeeId == id);
        }

        //GetPaged
        public async Task<(IEnumerable<Employee> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending)
        {
            var query = context.Employees
                .AsQueryable();

            // No Valid Search Columns Specified

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = sortColumn?.ToLower() switch
            {
                _ => query.OrderBy(a => a.EmployeeId)  // Default sorting
            };

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        //Create
        public async Task<Employee> AddAsync(Employee employee)
        {
            context.Employees.Add(employee);
            await context.SaveChangesAsync();
            return employee;
        }

        //Update
        public async Task<Employee> UpdateAsync(Employee employee)
        {
            try
            {
                var existingEmployee = await context.Employees
                    .FirstOrDefaultAsync(a => a.EmployeeId == employee.EmployeeId);

                if (existingEmployee == null)
                {
                    throw new Exception($"Employee with ID {employee.EmployeeId} not found");
                }

                // Update the existing entity's properties
                context.Entry(existingEmployee).CurrentValues.SetValues(employee);

                // Save changes
                await context.SaveChangesAsync();

                return existingEmployee;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                throw new Exception("The employee has been modified by another user. Please refresh and try again.", ex);
            }
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            var employee = await context.Employees.FindAsync(id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                await context.SaveChangesAsync();
            }
        }

    }
}
