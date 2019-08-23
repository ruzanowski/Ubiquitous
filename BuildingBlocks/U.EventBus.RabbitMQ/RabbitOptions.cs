namespace U.EventBus.RabbitMQ
{
    public class RabbitOptions
    {
        public bool Enabled { get; set; } = true;
        public string SubscriptionClientName { get; set; }
        public string EventBusConnection { get; set; }
        public string EventBusUserName { get; set; }
        public string EventBusPassword { get; set; }
        public string EventBusRetryCount { get; set; }
    }
}