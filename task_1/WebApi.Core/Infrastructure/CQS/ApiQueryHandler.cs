namespace WebApi.Core.Infrastructure.CQS;

public abstract class ApiQueryHandler<TQuery, TResult> : ApiRequestHandler<TQuery, TResult>,
    IQueryHandler<TQuery, TResult> where TQuery : ApiQuery<TResult>
{
    protected sealed override Task<TResult> HandleRequest(TQuery request, CancellationToken ct)
    {
        return Handle(request, ct);
    }

    public abstract Task<TResult> Handle(TQuery request, CancellationToken ct);
}