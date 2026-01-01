namespace Infrastructure.Repositories
{
    public class PlaylistRepository(ChinookWebAppContext context) : IPlaylistRepository
    {

        //GetAll
        public async Task<IEnumerable<Playlist>> GetAllAsync()
        {
            return await context.Playlists
                .ToListAsync();
        }

        //GetById
        public async Task<Playlist?> GetByIdAsync(int id)
        {
            return await context.Playlists
                .FirstOrDefaultAsync(a => a.PlaylistId == id);
        }

        //GetPaged
        public async Task<(IEnumerable<Playlist> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending)
        {
            var query = context.Playlists
                .AsQueryable();

            // No Valid Search Columns Specified

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = sortColumn?.ToLower() switch
            {
                _ => query.OrderBy(a => a.PlaylistId)  // Default sorting
            };

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        //Create
        public async Task<Playlist> AddAsync(Playlist playlist)
        {
            context.Playlists.Add(playlist);
            await context.SaveChangesAsync();
            return playlist;
        }

        //Update
        public async Task<Playlist> UpdateAsync(Playlist playlist)
        {
            try
            {
                var existingPlaylist = await context.Playlists
                    .FirstOrDefaultAsync(a => a.PlaylistId == playlist.PlaylistId);

                if (existingPlaylist == null)
                {
                    throw new Exception($"Playlist with ID {playlist.PlaylistId} not found");
                }

                // Update the existing entity's properties
                context.Entry(existingPlaylist).CurrentValues.SetValues(playlist);

                // Save changes
                await context.SaveChangesAsync();

                return existingPlaylist;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                throw new Exception("The playlist has been modified by another user. Please refresh and try again.", ex);
            }
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            var playlist = await context.Playlists.FindAsync(id);
            if (playlist != null)
            {
                context.Playlists.Remove(playlist);
                await context.SaveChangesAsync();
            }
        }

    }
}
