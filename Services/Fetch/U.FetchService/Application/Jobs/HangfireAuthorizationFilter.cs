using Hangfire.Dashboard;
using Hangfire.PostgreSql.Annotations;

namespace U.FetchService.Application.Jobs
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            //can add some more logic here...
            return true;
        }
    }
}