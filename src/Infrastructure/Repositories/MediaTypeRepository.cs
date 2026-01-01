namespace Infrastructure.Repositories
{
    public class MediaTypeRepository(ChinookWebAppContext context) : IMediaTypeRepository
    {

        //GetAll
        public async Task<IEnumerable<MediaType>> GetAllAsync()
        {
            return await context.MediaTypes
                .ToListAsync();
        }

        //GetById
        public async Task<MediaType?> GetByIdAsync(int id)
        {
            return await context.MediaTypes
                .FirstOrDefaultAsync(a => a.MediaTypeId == id);
        }

        //GetPaged
        public async Task<(IEnumerable<MediaType> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending)
        {
            var query = context.MediaTypes
                .AsQueryable();

            // No Valid Search Columns Specified

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = sortColumn?.ToLower() switch
            {
                _ => query.OrderBy(a => a.MediaTypeId)  // Default sorting
            };

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        //Create
        public async Task<MediaType> AddAsync(MediaType mediatype)
        {
            context.MediaTypes.Add(mediatype);
            await context.SaveChangesAsync();
            return mediatype;
        }

        //Update
        public async Task<MediaType> UpdateAsync(MediaType mediatype)
        {
            try
            {
                var existingMediaType = await context.MediaTypes
                    .FirstOrDefaultAsync(a => a.MediaTypeId == mediatype.MediaTypeId);

                if (existingMediaType == null)
                {
                    throw new Exception($"MediaType with ID {mediatype.MediaTypeId} not found");
                }

                // Update the existing entity's properties
                context.Entry(existingMediaType).CurrentValues.SetValues(mediatype);

                // Save changes
                await context.SaveChangesAsync();

                return existingMediaType;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                throw new Exception("The mediatype has been modified by another user. Please refresh and try again.", ex);
            }
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            var mediatype = await context.MediaTypes.FindAsync(id);
            if (mediatype != null)
            {
                context.MediaTypes.Remove(mediatype);
                await context.SaveChangesAsync();
            }
        }

    }
}
