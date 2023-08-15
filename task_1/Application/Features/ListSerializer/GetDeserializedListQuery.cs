using Application.Dto;
using Microsoft.AspNetCore.Http;
using WebApi.Core.Infrastructure.CQS;

namespace Application.Features.ListSerializer;

public class GetDeserializedListQuery : ApiQuery<List<ListNodeDto>>
{
    public IFormFile File { get; set; }
}