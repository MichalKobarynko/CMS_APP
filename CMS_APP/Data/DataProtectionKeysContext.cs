using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CMS_APP.Data
{
    public class DataProtectionKeysContext : DbContext, IDataProtectionKeyContext
    {
        public DataProtectionKeysContext(DbContextOptions<DataProtectionKeysContext> options) 
            : base(options)
        {

        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    }
}
