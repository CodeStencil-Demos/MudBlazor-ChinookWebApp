using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.Data.Configurations;

namespace Infrastructure.Data
{
    public partial class ChinookWebAppContext(DbContextOptions<ChinookWebAppContext> options) : IdentityDbContext(options)
    {

    }
}
