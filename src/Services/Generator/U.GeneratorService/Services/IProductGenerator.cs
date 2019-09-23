using System.Threading.Tasks;

namespace U.GeneratorService.Services
{
    public interface IProductGenerator
    {
        SmartProductDto GenerateFakeProduct();
    }
}