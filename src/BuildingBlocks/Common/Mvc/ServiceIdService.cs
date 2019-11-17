using System;

namespace U.Common.Mvc
{
    public class ServiceIdService : IServiceIdService
    {
        private static readonly string UniqueId = $"{Guid.NewGuid():N}";

        public string Id => UniqueId;
    }
}