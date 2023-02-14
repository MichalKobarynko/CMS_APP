using CMS_APP.FunctionalServices;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_APP.Data
{
    public static class DbContextInitializer
    {
        public static async Task Initializer(
            DataProtectionKeysContext dataProtectionKeysContext,
            ApplicationDbContext applicationDbContext,
            IFunctionalSvc functionalService)
        {
            await dataProtectionKeysContext.Database.EnsureCreatedAsync();
            await applicationDbContext.Database.EnsureCreatedAsync();

            if(applicationDbContext.ApplicationUsers.Any())
                return;

            await functionalService.CreateDefaultAdminUser();
            await functionalService.CreateDefaultAppUser();
            
        }
    }
}
