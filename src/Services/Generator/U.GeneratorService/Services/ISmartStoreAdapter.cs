using System.Threading.Tasks;
using RestEase;

namespace U.GeneratorService.Services
 {
     public interface ISmartStoreAdapter
     {
         [AllowAnyStatusCode]
         [Post("api/smartstore/products/store")]
         Task StoreProduct([Body] SmartProductDto dto);
     }
 }