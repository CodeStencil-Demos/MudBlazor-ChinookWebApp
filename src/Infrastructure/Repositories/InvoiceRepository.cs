namespace Infrastructure.Repositories
{
    public class InvoiceRepository(ChinookWebAppContext context) : IInvoiceRepository
    {

        //GetAll
        public async Task<IEnumerable<Invoice>> GetAllAsync()
        {
            return await context.Invoices
                .Include(a => a.Customer)
                .ToListAsync();
        }

        //GetById
        public async Task<Invoice?> GetByIdAsync(int id)
        {
            return await context.Invoices
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(a => a.InvoiceId == id);
        }

        //GetPaged
        public async Task<(IEnumerable<Invoice> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending)
        {
            var query = context.Invoices
                .Include(a => a.Customer)
                .AsQueryable();

            // No Valid Search Columns Specified

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = sortColumn?.ToLower() switch
            {
                "customer" => isDescending
                    ? query.OrderByDescending(a => a.Customer != null ? a.Customer.FirstName : string.Empty)
                    : query.OrderBy(a => a.Customer != null ? a.Customer.FirstName : string.Empty),
                _ => query.OrderBy(a => a.InvoiceId)  // Default sorting
            };

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        //Create
        public async Task<Invoice> AddAsync(Invoice invoice)
        {
            context.Invoices.Add(invoice);
            await context.SaveChangesAsync();
            return invoice;
        }

        //Update
        public async Task<Invoice> UpdateAsync(Invoice invoice)
        {
            try
            {
                var existingInvoice = await context.Invoices
                    .FirstOrDefaultAsync(a => a.InvoiceId == invoice.InvoiceId);

                if (existingInvoice == null)
                {
                    throw new Exception($"Invoice with ID {invoice.InvoiceId} not found");
                }

                // Update the existing entity's properties
                context.Entry(existingInvoice).CurrentValues.SetValues(invoice);

                // Save changes
                await context.SaveChangesAsync();

                return existingInvoice;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                throw new Exception("The invoice has been modified by another user. Please refresh and try again.", ex);
            }
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            var invoice = await context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                context.Invoices.Remove(invoice);
                await context.SaveChangesAsync();
            }
        }

    }
}
