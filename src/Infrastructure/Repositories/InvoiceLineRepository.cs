namespace Infrastructure.Repositories
{
    public class InvoiceLineRepository(ChinookWebAppContext context) : IInvoiceLineRepository
    {

        //GetAll
        public async Task<IEnumerable<InvoiceLine>> GetAllAsync()
        {
            return await context.InvoiceLines
                .Include(a => a.Invoice)
                .Include(a => a.Track)
                .ToListAsync();
        }

        //GetById
        public async Task<InvoiceLine?> GetByIdAsync(int id)
        {
            return await context.InvoiceLines
                .Include(a => a.Invoice)
                .Include(a => a.Track)
                .FirstOrDefaultAsync(a => a.InvoiceLineId == id);
        }

        //GetPaged
        public async Task<(IEnumerable<InvoiceLine> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending)
        {
            var query = context.InvoiceLines
                .Include(a => a.Invoice)
                .Include(a => a.Track)
                .AsQueryable();

            // No Valid Search Columns Specified

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = sortColumn?.ToLower() switch
            {
                "invoice" => isDescending
                    ? query.OrderByDescending(a => a.Invoice != null ? a.Invoice.BillingAddress : string.Empty)
                    : query.OrderBy(a => a.Invoice != null ? a.Invoice.BillingAddress : string.Empty),
                _ => query.OrderBy(a => a.InvoiceLineId)  // Default sorting
            };

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        //Create
        public async Task<InvoiceLine> AddAsync(InvoiceLine invoiceline)
        {
            context.InvoiceLines.Add(invoiceline);
            await context.SaveChangesAsync();
            return invoiceline;
        }

        //Update
        public async Task<InvoiceLine> UpdateAsync(InvoiceLine invoiceline)
        {
            try
            {
                var existingInvoiceLine = await context.InvoiceLines
                    .FirstOrDefaultAsync(a => a.InvoiceLineId == invoiceline.InvoiceLineId);

                if (existingInvoiceLine == null)
                {
                    throw new Exception($"InvoiceLine with ID {invoiceline.InvoiceLineId} not found");
                }

                // Update the existing entity's properties
                context.Entry(existingInvoiceLine).CurrentValues.SetValues(invoiceline);

                // Save changes
                await context.SaveChangesAsync();

                return existingInvoiceLine;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                throw new Exception("The invoiceline has been modified by another user. Please refresh and try again.", ex);
            }
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            var invoiceline = await context.InvoiceLines.FindAsync(id);
            if (invoiceline != null)
            {
                context.InvoiceLines.Remove(invoiceline);
                await context.SaveChangesAsync();
            }
        }

    }
}
