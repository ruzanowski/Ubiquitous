using System.Threading.Tasks;
using RestEase;
using U.Common;
using U.Common.Miscellaneous;
using U.Common.Products;

namespace U.GeneratorService.Services
 {
     public interface ISmartStoreAdapter
     {
         [AllowAnyStatusCode]
         [Post(GlobalConstants.SmartStoreStoreProductPath)]
         Task StoreProduct([Body] SmartProductDto dto);
     }
 }