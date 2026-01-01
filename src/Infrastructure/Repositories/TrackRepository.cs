namespace Infrastructure.Repositories
{
    public class TrackRepository(ChinookWebAppContext context) : ITrackRepository
    {

        //GetAll
        public async Task<IEnumerable<Track>> GetAllAsync()
        {
            return await context.Tracks
                .Include(a => a.Album)
                .Include(a => a.MediaType)
                .Include(a => a.Genre)
                .ToListAsync();
        }

        //GetById
        public async Task<Track?> GetByIdAsync(int id)
        {
            return await context.Tracks
                .Include(a => a.Album)
                .Include(a => a.MediaType)
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(a => a.TrackId == id);
        }

        //GetPaged
        public async Task<(IEnumerable<Track> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending)
        {
            var query = context.Tracks
                .Include(a => a.Album)
                .Include(a => a.MediaType)
                .Include(a => a.Genre)
                .AsQueryable();

            // No Valid Search Columns Specified

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = sortColumn?.ToLower() switch
            {
                "album" => isDescending
                    ? query.OrderByDescending(a => a.Album != null ? a.Album.Title : string.Empty)
                    : query.OrderBy(a => a.Album != null ? a.Album.Title : string.Empty),
                _ => query.OrderBy(a => a.TrackId)  // Default sorting
            };

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        //Create
        public async Task<Track> AddAsync(Track track)
        {
            context.Tracks.Add(track);
            await context.SaveChangesAsync();
            return track;
        }

        //Update
        public async Task<Track> UpdateAsync(Track track)
        {
            try
            {
                var existingTrack = await context.Tracks
                    .FirstOrDefaultAsync(a => a.TrackId == track.TrackId);

                if (existingTrack == null)
                {
                    throw new Exception($"Track with ID {track.TrackId} not found");
                }

                // Update the existing entity's properties
                context.Entry(existingTrack).CurrentValues.SetValues(track);

                // Save changes
                await context.SaveChangesAsync();

                return existingTrack;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                throw new Exception("The track has been modified by another user. Please refresh and try again.", ex);
            }
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            var track = await context.Tracks.FindAsync(id);
            if (track != null)
            {
                context.Tracks.Remove(track);
                await context.SaveChangesAsync();
            }
        }

    }
}
