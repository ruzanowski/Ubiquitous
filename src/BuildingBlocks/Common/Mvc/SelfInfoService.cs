using System;

namespace U.Common.Mvc
{
    public class SelfInfoService : ISelfInfoService
    {
        private static readonly string UniqueId = $"{Guid.NewGuid():N}";

        public string Id => UniqueId;
    }
}