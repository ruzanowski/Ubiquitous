using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using U.FetchService.Application.Commands.Dispatch;

namespace U.FetchService.Application.Jobs
{
    public static class JobsInstaller
    {
        public static void UseCustomBackgroundJobs(this IApplicationBuilder app)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate<HangfireMediator>(job => job.SendCommand(new DispatchCommand
                {
                    Executor = "Hangfire",
                    ExecutorComment = "Recurring job"
                }),
                "*/10 * * * * *",
                TimeZoneInfo.Utc);
        }
    }
}