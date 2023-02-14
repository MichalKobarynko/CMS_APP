using System.Threading.Tasks;

namespace CMS_APP.FunctionalServices
{
    public interface IFunctionalSvc
    {
        Task CreateDefaultAdminUser();
        Task CreateDefaultAppUser();
    }
}
