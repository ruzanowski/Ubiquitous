using MediatR;

namespace U.FetchService.Application.Operations.Dispatch
{
    public class DispatchCommand : IRequest
    {
        public string Executor { get; set; }
        public string ExecutorComment { get; set; }
    }
}