using System;
using Microsoft.EntityFrameworkCore;
// ReSharper disable All

namespace U.FetchService.Domain.Entities
{
    [Owned]
    public class Executed
    {
        private Executed()
        {
            
        }
        private Executed(string executedBy, string executorComment)
        {
            At = DateTime.UtcNow;
            By = executedBy;
            ExecutorComment = executorComment;
        }

        public DateTime At { get; protected set; }
        public string By { get; protected set; }
        public string ExecutorComment { get; protected set; }

        public static class Factory
        {
            public static Executed Create(string executedBy, string executorComment)
            {
                return new Executed(executedBy, executorComment);
            }
        }
    }
}