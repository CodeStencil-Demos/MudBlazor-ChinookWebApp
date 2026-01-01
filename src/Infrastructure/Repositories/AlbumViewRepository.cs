namespace Infrastructure.Repositories
{
    public class AlbumViewRepository(ChinookWebAppContext context) : IAlbumViewRepository
    {

        //GetAll
        public async Task<IEnumerable<AlbumView>> GetAllAsync()
        {
            return await context.AlbumViews.ToListAsync();
        }

        //Search
        public async Task<IEnumerable<AlbumView>> SearchAsync(string searchTerm)
        {
            // No searchable columns specified for this view
            throw new NotImplementedException();
        }


        //GetPaged
        public async Task<(IEnumerable<AlbumView> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending)
        {
            var query = context.AlbumViews
                .AsQueryable();

            // No Valid Search Columns Specified

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = sortColumn?.ToLower() switch
            {
                _ => query.OrderBy(a => a.Title)  // Default sorting
            };

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

    }
}
