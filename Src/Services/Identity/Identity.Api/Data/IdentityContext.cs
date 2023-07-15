using Identity.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Data
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; private init; }
    }
}
