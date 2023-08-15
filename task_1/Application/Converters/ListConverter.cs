using Application.Dto;
using SerializerTests.Nodes;

namespace Application.Converters;

public static class ListConverter
{
    public static ListNode? ConvertToLinkedList(List<ListNodeDto> dtoList)
    {
        var size = dtoList.Count;
        if (size == 0)
        {
            return null;
        }

        var nodeMapping = new Dictionary<int, ListNode>();

        for (var i = 0; i < size; i++)
        {
            var dto = dtoList[i];
            var newNode = new ListNode { Data = dto.Data };
            nodeMapping[i] = newNode;
        }

        for (var i = 0; i < size; i++)
        {
            var dto = dtoList[i];
            var currentNode = nodeMapping[i];
            
            currentNode.Next = i < size - 1 ? nodeMapping[i + 1] : null;
            currentNode.Previous = i > 0 ? nodeMapping[i - 1] : null;
            if (dto.RandomIndex != -1)
            {
                currentNode.Random = nodeMapping[dto.RandomIndex];
            }
        }

        return nodeMapping[0];
    }
    
    public static List<ListNodeDto> ConvertToDtoList(ListNode head)
    {
        var dtoList = new List<ListNodeDto>();
        var nodeMapping = new Dictionary<ListNode, int>();

        var currentNode = head;
        var index = 0;

        while (currentNode is not null)
        {
            nodeMapping[currentNode] = index++;
            currentNode = currentNode.Next;
        }

        currentNode = head;

        while (currentNode is not null)
        {
            var randomIndex = currentNode.Random is not null ? nodeMapping[currentNode.Random] : -1;

            dtoList.Add(new ListNodeDto
            {
                RandomIndex = randomIndex,
                Data = currentNode.Data
            });

            currentNode = currentNode.Next;
        }

        return dtoList;
    }
}