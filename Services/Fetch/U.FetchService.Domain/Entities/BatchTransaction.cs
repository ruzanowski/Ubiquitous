using System;
// ReSharper disable All

namespace U.FetchService.Domain.Entities
{
    public class BatchTransaction
    {
        private BatchTransaction()
        {
        }

        /// <summary>
        /// Id of transaction.
        /// Transaction count for one fetch from single Party.
        /// </summary>
        public Guid Id { get; protected set; }

        public int ItemsCount { get; protected set; }
        public Executed Executed { get; protected set; }
        public Party From { get; protected set; }
        public Party To { get; protected set; }

        public static class Factory
        {
            public static BatchTransaction Create(Executed executed, Party fromParty, Party toParty, int itemsCount)
            {
                return new BatchTransaction
                {
                    Executed = executed,
                    From = fromParty,
                    To = toParty,
                    ItemsCount = itemsCount
                };
            }
        }
    }
}