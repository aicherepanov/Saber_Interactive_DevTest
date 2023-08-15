using Application.Converters;
using Application.Dto;
using SerializerTests.Interfaces;
using WebApi.Core.Infrastructure.CQS;

namespace Application.Features.ListSerializer.Handlers;

public class GetDeserializedListQueryHandler : ApiQueryHandler<GetDeserializedListQuery, List<ListNodeDto>>
{
    private readonly IListSerializer _listSerializer;
    
    public GetDeserializedListQueryHandler(IListSerializer listSerializer)
    {
        _listSerializer = listSerializer;
    }

    public override async Task<List<ListNodeDto>> Handle(GetDeserializedListQuery request, CancellationToken ct)
    {
        if (request.File.Length == 0)
        {
            throw new Exception("Файл пустой");
        }

        using var memoryStream = new MemoryStream();
        await request.File.CopyToAsync(memoryStream, ct);
        memoryStream.Seek(0, SeekOrigin.Begin);

        var head = await _listSerializer.Deserialize(memoryStream);

        var nodeDtoList = ListConverter.ConvertToDtoList(head);
        return nodeDtoList;
    }
}