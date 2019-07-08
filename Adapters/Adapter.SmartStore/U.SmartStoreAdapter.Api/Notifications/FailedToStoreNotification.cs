using System;
using MediatR;
using U.Common;

namespace U.SmartStoreAdapter.Api.Notifications
{
    public class FailedToStoreNotification<TIn,TOut> : INotification
    { /// <summary>
      /// Entity Name e.g: Product, Category, Manufacturer
      /// </summary>
        public string EntityName { get; set;  }
      
        public DataTransaction<TIn,TOut> DataTransaction { get; set; }
        public Exception Error { get; set; }
    }
}