namespace Infrastructure.Repositories
{
    public class GenreRepository(ChinookWebAppContext context) : IGenreRepository
    {

        //GetAll
        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await context.Genres
                .ToListAsync();
        }

        //GetById
        public async Task<Genre?> GetByIdAsync(int id)
        {
            return await context.Genres
                .FirstOrDefaultAsync(a => a.GenreId == id);
        }

        //GetPaged
        public async Task<(IEnumerable<Genre> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending)
        {
            var query = context.Genres
                .AsQueryable();

            // No Valid Search Columns Specified

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = sortColumn?.ToLower() switch
            {
                _ => query.OrderBy(a => a.GenreId)  // Default sorting
            };

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        //Create
        public async Task<Genre> AddAsync(Genre genre)
        {
            context.Genres.Add(genre);
            await context.SaveChangesAsync();
            return genre;
        }

        //Update
        public async Task<Genre> UpdateAsync(Genre genre)
        {
            try
            {
                var existingGenre = await context.Genres
                    .FirstOrDefaultAsync(a => a.GenreId == genre.GenreId);

                if (existingGenre == null)
                {
                    throw new Exception($"Genre with ID {genre.GenreId} not found");
                }

                // Update the existing entity's properties
                context.Entry(existingGenre).CurrentValues.SetValues(genre);

                // Save changes
                await context.SaveChangesAsync();

                return existingGenre;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                throw new Exception("The genre has been modified by another user. Please refresh and try again.", ex);
            }
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            var genre = await context.Genres.FindAsync(id);
            if (genre != null)
            {
                context.Genres.Remove(genre);
                await context.SaveChangesAsync();
            }
        }

    }
}
