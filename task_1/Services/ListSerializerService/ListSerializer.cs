using System.Text;
using SerializerTests.Interfaces;
using SerializerTests.Nodes;

namespace SerializerTests.Implementations;

public class ListSerializer : IListSerializer
{
    public ListSerializer()
    {
    }
    
    /// <inheritdoc />
    public async Task Serialize(ListNode head, Stream s)
    {
        await using var writer = new BinaryWriter(s, Encoding.UTF8, leaveOpen: true);
        var dataBuilder = new StringBuilder();
        var randomLinksBuilder = new StringBuilder();

        var nodeMapping = new Dictionary<ListNode, int>();

        head = await SerializeDataNodes(head, dataBuilder, nodeMapping);
        WriteData(dataBuilder.ToString(), writer);

        await SerializeRandomLinks(head, randomLinksBuilder, nodeMapping);
        WriteData(randomLinksBuilder.ToString(), writer);
    }
    
    /// <inheritdoc />
    public Task<ListNode> Deserialize(Stream s)
    {
        using var reader = new BinaryReader(s, Encoding.UTF8, leaveOpen: true);
        
        var dataLength = reader.ReadInt32();
        var dataBytes = reader.ReadBytes(dataLength);
        var data = Encoding.UTF8.GetString(dataBytes);
        
        var randomLinksLength = reader.ReadInt32();
        var randomLinksBytes = reader.ReadBytes(randomLinksLength);
        var randomLinks = Encoding.UTF8.GetString(randomLinksBytes);

        var dataReader = new StringReader(data);
        var randomLinksReader = new StringReader(randomLinks);

        var nodeMapping = new Dictionary<int, ListNode>();

        SetNodeMapping(dataReader, nodeMapping);
        var head = DeserializeLinks(nodeMapping);
        DeserializeRandomLinks(randomLinksReader, nodeMapping);

        return Task.FromResult(head);
    }
    
    /// <inheritdoc />
    public async Task<ListNode> DeepCopy(ListNode head)
    {
        using var memoryStream = new MemoryStream();
        await Serialize(head, memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return await Deserialize(memoryStream);
    }
    
    private Task<ListNode> SerializeDataNodes(ListNode node,
        StringBuilder dataBuilder,
        Dictionary<ListNode, int> nodeMapping)
    {
        var head = node;
        var currentIndex = 0;

        while (node is not null)
        {
            nodeMapping[node] = currentIndex;
            dataBuilder.AppendLine($"{currentIndex} {node.Data}");
            currentIndex++;
            node = node.Next;
        }

        return Task.FromResult(head);
    }

    private async Task SerializeRandomLinks(ListNode node,
        StringBuilder randomLinksBuilder,
        Dictionary<ListNode, int> nodeMapping)
    {
        var tasks = new List<Task>();

        while (node is not null)
        {
            var currentNode = node;
            tasks.Add(Task.Run(() =>
            {
                lock (randomLinksBuilder)
                {
                    var randomIndex = currentNode.Random is not null
                        ? nodeMapping[currentNode.Random].ToString()
                        : "-1";
                    randomLinksBuilder.AppendLine($"{nodeMapping[currentNode]} {randomIndex}");
                }
            }));
            node = node.Next;
        }

        await Task.WhenAll(tasks);
    }

    private void WriteData(string data, BinaryWriter writer)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);
        writer.Write(BitConverter.GetBytes(dataBytes.Length));
        writer.Write(dataBytes);
    }
    
    private void SetNodeMapping(StringReader reader, Dictionary<int, ListNode> nodeMapping)
    {
        while (reader.ReadLine() is { } dataLine)
        {
            var parts = dataLine.Split(' ', 2);
            var nodeIndex = int.Parse(parts[0]);
            var data = parts[1];
            nodeMapping[nodeIndex] = new ListNode { Data = data };
        }
    }

    private ListNode DeserializeLinks(Dictionary<int, ListNode> nodeMapping)
    {
        var nodeIndex = 0;
        
        var head = nodeMapping[nodeIndex];
        var currentNode = head;
        while (currentNode is not null)
        {
            if (!nodeMapping.ContainsKey(++nodeIndex))
            {
                break;
            }

            var nextNode = nodeMapping[nodeIndex];
            currentNode.Next = nextNode;
            nextNode.Previous = currentNode;
            currentNode = nextNode;
        }

        return head;
    }
    
    private void DeserializeRandomLinks(StringReader reader, Dictionary<int, ListNode> nodeMapping)
    {
        while (reader.ReadLine() is { } randomLine)
        {
            var parts = randomLine.Split(' ');
            var dataIndex = int.Parse(parts[0]);
            var randomIndex = int.Parse(parts[1]);
            if (randomIndex != -1)
            {
                var node = nodeMapping[dataIndex];
                node.Random = nodeMapping[randomIndex];
            }
        }
    }
}