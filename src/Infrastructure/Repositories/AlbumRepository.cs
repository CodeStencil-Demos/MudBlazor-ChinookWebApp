namespace Infrastructure.Repositories
{
    public class AlbumRepository(ChinookWebAppContext context) : IAlbumRepository
    {

        //GetAll
        public async Task<IEnumerable<Album>> GetAllAsync()
        {
            return await context.Albums
                .Include(a => a.Artist)
                .ToListAsync();
        }

        //GetById
        public async Task<Album?> GetByIdAsync(int id)
        {
            return await context.Albums
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(a => a.AlbumId == id);
        }

        //GetPaged
        public async Task<(IEnumerable<Album> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending)
        {
            var query = context.Albums
                .Include(a => a.Artist)
                .AsQueryable();

            // No Valid Search Columns Specified

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = sortColumn?.ToLower() switch
            {
                "artist" => isDescending
                    ? query.OrderByDescending(a => a.Artist != null ? a.Artist.Name : string.Empty)
                    : query.OrderBy(a => a.Artist != null ? a.Artist.Name : string.Empty),
                _ => query.OrderBy(a => a.AlbumId)  // Default sorting
            };

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        //Create
        public async Task<Album> AddAsync(Album album)
        {
            context.Albums.Add(album);
            await context.SaveChangesAsync();
            return album;
        }

        //Update
        public async Task<Album> UpdateAsync(Album album)
        {
            try
            {
                var existingAlbum = await context.Albums
                    .FirstOrDefaultAsync(a => a.AlbumId == album.AlbumId);

                if (existingAlbum == null)
                {
                    throw new Exception($"Album with ID {album.AlbumId} not found");
                }

                // Update the existing entity's properties
                context.Entry(existingAlbum).CurrentValues.SetValues(album);

                // Save changes
                await context.SaveChangesAsync();

                return existingAlbum;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                throw new Exception("The album has been modified by another user. Please refresh and try again.", ex);
            }
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            var album = await context.Albums.FindAsync(id);
            if (album != null)
            {
                context.Albums.Remove(album);
                await context.SaveChangesAsync();
            }
        }

    }
}
