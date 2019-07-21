using System;
using Hangfire;
using MediatR;

namespace U.FetchService.Application.Jobs
{
    public class MediatRJobActivator : JobActivator
    {
        private readonly IMediator _mediator;

        public MediatRJobActivator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override object ActivateJob(Type type)
        {
            return new HangfireMediator(_mediator);
        }
    }
}