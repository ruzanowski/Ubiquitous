using System;

namespace U.SmartStoreAdapter.Application.Models.DataRequests
{
    public class DataTransaction<TIn, TOut>
    {
        private DataTransaction()
        {
        }

        public TIn RequestData { get; private set; }
        public TOut ResultData { get; private set; }
        public DateTime Requested { get; private set; }
        public string TransactionId { get; private set; }
        public bool RolledBack { get; private set; }

        public static DataTransaction<TIn, TOut> Create(DateTime dateTime,
            TIn requestData,
            TOut resultData,
            string transactionId,
            bool rolledBack)
        {
            return new DataTransaction<TIn, TOut>
            {
                Requested = dateTime,
                RequestData = requestData,
                ResultData = resultData,
                RolledBack = rolledBack,
                TransactionId = transactionId
            };
        }
    }
}