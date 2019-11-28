using Newtonsoft.Json;

namespace U.EventBus.Events.Identity
{
    public class SignInRejected : IntegrationEvent
    {
        public string Email { get; }
        public string Reason { get; }
        public string Code { get; }

        [JsonConstructor]
        public SignInRejected(string email, string reason, string code)
        {
            Email = email;
            Reason = reason;
            Code = code;
        }
    }
}