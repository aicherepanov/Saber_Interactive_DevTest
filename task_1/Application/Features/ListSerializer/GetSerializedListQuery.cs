using Application.Dto;
using WebApi.Core.Infrastructure.CQS;

namespace Application.Features.ListSerializer;

public class GetSerializedListQuery : ApiQuery<byte[]>
{
    public List<ListNodeDto> Nodes { get; set; }
}