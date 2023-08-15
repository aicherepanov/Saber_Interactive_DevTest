using Application.Dto;
using Application.Features.ListSerializer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ListSerializer.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ListSerializerController : Controller
{
    private readonly IMediator _mediator;
    
    public ListSerializerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("serialize")]
    public async Task<IActionResult> SerializeList([FromBody] GetSerializedListQuery query, CancellationToken ct)
    {
        var bytes = await _mediator.Send(query, ct);
        var result = new FileContentResult(bytes, "application/octet-stream")
            { FileDownloadName = "serialized_list.bin" };
        return result;
    }

    [HttpPost("deserialize")]
    public async Task<List<ListNodeDto>> DeserializeList([FromForm] GetDeserializedListQuery query,
        CancellationToken ct)
    {
        return await _mediator.Send(query, ct);
    }
}