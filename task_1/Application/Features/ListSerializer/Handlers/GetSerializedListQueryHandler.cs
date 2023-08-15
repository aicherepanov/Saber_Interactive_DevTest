using Application.Converters;
using SerializerTests.Interfaces;
using WebApi.Core.Infrastructure.CQS;

namespace Application.Features.ListSerializer.Handlers;

public class GetSerializedListQueryHandler : ApiQueryHandler<GetSerializedListQuery, byte[]>
{
    private readonly IListSerializer _listSerializer;
    
    public GetSerializedListQueryHandler(IListSerializer listSerializer)
    {
        _listSerializer = listSerializer;
    }

    public override async Task<byte[]> Handle(GetSerializedListQuery request, CancellationToken ct)
    {
        if (request.Nodes.Count == 0)
        {
            throw new Exception("Список пустой. Необходимо добавить узлы в список.");
        }

        var listNode = ListConverter.ConvertToLinkedList(request.Nodes);

        if (listNode is null)
        {
            throw new Exception("Не удалось конвертировать List<ListNodeDto> в список ListNode");
        }
        
        using var memoryStream = new MemoryStream();
        await _listSerializer.Serialize(listNode, memoryStream);
        return memoryStream.ToArray();
    }
}