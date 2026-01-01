namespace Infrastructure.Repositories
{
    public class CustomerRepository(ChinookWebAppContext context) : ICustomerRepository
    {

        //GetAll
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await context.Customers
                .Include(a => a.SupportRep)
                .ToListAsync();
        }

        //GetById
        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await context.Customers
                .Include(a => a.SupportRep)
                .FirstOrDefaultAsync(a => a.CustomerId == id);
        }

        //GetPaged
        public async Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending)
        {
            var query = context.Customers
                .Include(a => a.SupportRep)
                .AsQueryable();

            // No Valid Search Columns Specified

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = sortColumn?.ToLower() switch
            {
                "supportrep" => isDescending
                    ? query.OrderByDescending(a => a.SupportRep != null ? a.SupportRep.LastName : string.Empty)
                    : query.OrderBy(a => a.SupportRep != null ? a.SupportRep.LastName : string.Empty),
                _ => query.OrderBy(a => a.CustomerId)  // Default sorting
            };

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        //Create
        public async Task<Customer> AddAsync(Customer customer)
        {
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            return customer;
        }

        //Update
        public async Task<Customer> UpdateAsync(Customer customer)
        {
            try
            {
                var existingCustomer = await context.Customers
                    .FirstOrDefaultAsync(a => a.CustomerId == customer.CustomerId);

                if (existingCustomer == null)
                {
                    throw new Exception($"Customer with ID {customer.CustomerId} not found");
                }

                // Update the existing entity's properties
                context.Entry(existingCustomer).CurrentValues.SetValues(customer);

                // Save changes
                await context.SaveChangesAsync();

                return existingCustomer;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                throw new Exception("The customer has been modified by another user. Please refresh and try again.", ex);
            }
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            var customer = await context.Customers.FindAsync(id);
            if (customer != null)
            {
                context.Customers.Remove(customer);
                await context.SaveChangesAsync();
            }
        }

    }
}
