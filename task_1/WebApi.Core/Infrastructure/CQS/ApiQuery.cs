using MediatR;

namespace WebApi.Core.Infrastructure.CQS;

public class ApiQuery<TResult> : IQuery, IRequest<TResult>
{
}