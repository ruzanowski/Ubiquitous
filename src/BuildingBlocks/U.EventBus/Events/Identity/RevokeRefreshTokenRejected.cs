using System;
using Newtonsoft.Json;

namespace U.EventBus.Events.Identity
{
    public class RevokeRefreshTokenRejected : IntegrationEvent
    {
        public Guid UserId { get; }
        public string Reason { get; }
        public string Code { get; }

        [JsonConstructor]
        public RevokeRefreshTokenRejected(Guid userId, string reason, string code)
        {
            UserId = userId;
            Reason = reason;
            Code = code;
        }
    }
}