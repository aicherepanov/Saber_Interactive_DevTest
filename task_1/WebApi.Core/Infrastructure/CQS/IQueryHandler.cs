namespace WebApi.Core.Infrastructure.CQS;

public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery
{
    Task<TResult> Handle(TQuery request, CancellationToken ct);
}