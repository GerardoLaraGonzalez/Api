using HeladoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HeladoApi.Data
{
    public class HeladeriaDb : DbContext
    {
        public HeladeriaDb(DbContextOptions<HeladeriaDb> options) : base(options)
        {
            
        }
        public DbSet<IcecreamApi> HeladoApis => Set<IcecreamApi>();



    }
}
