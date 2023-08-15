using MediatR;

namespace WebApi.Core.Infrastructure.CQS;

public abstract class ApiRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    async Task<TResponse> IRequestHandler<TRequest, TResponse>.Handle(TRequest request, CancellationToken ct)
    {
        var result = await CoreHandleRequest(request, ct);
        return result;
    }

    protected abstract Task<TResponse> HandleRequest(TRequest request, CancellationToken ct);

    protected virtual async Task<TResponse> CoreHandleRequest(TRequest request, CancellationToken ct)
    {
        await CanHandle(request, ct);
        return await HandleRequest(request, ct);
    }

    protected virtual Task CanHandle(TRequest request, CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}