using MediatR;
using U.Common;

namespace U.SmartStoreAdapter.Api.Notifications
{
    public class SuccessfulStoreNotification<TIn,TOut> : INotification
    {
        public string EntityName { get; set;  }
      
        public DataTransaction<TIn,TOut> DataTransaction { get; set; }
    }
}