using System;
using System.Reflection;

namespace U.Common.NetCore.Mvc
{
    public class SelfInfoService : ISelfInfoService
    {
        public SelfInfoService()
        {
            Id = NewGuid;
            Name = Assembly.GetEntryAssembly()?.GetName().Name;
        }

        private static string NewGuid => $"{Guid.NewGuid():N}";

        public string Id { get; }
        public string Name { get; }
    }
}