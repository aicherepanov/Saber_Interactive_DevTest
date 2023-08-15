using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SerializerTests.Interfaces;
using SerializerTests.Nodes;

namespace Services.UnitTests;

[TestClass]
public class ListSerializerTests : BaseTest
{
    private static IEnumerable<object[]> GetRandomLinkedLists()
    {
        yield return new object[]
        {
            TestDataGenerator.GenerateRandomLinkedList(10)
        };
        yield return new object[]
        {
            TestDataGenerator.GenerateRandomLinkedList(100)
        };
        yield return new object[]
        {
            TestDataGenerator.GenerateRandomLinkedList(1000)
        };
    }
    
    [TestMethod]
    [DynamicData(nameof(GetRandomLinkedLists), DynamicDataSourceType.Method)]
    public async Task DeepCopy_WithRandomLinkedList_ShouldReturnExactCopy(ListNode linkedList)
    {
        // Arrange
        var serializer = ServiceProvider!.GetRequiredService<IListSerializer>();

        // Act
        var deepCopyHead = await serializer.DeepCopy(linkedList);

        // Assert
        CompareLinkedLists(linkedList, deepCopyHead);
    }
    
    [TestMethod]
    public async Task Serialize_ShouldWriteListNodesInStreamCorrectly()
    {
        // Arrange
        var serializer = ServiceProvider!.GetRequiredService<IListSerializer>();
        var node1 = new ListNode { Data = "Node 1" };
        var node2 = new ListNode { Data = "Node 2" };
        node1.Next = node2;
        node2.Previous = node1;

        // Act
        using var memoryStream = new MemoryStream();
        await serializer.Serialize(node1, memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(memoryStream, Encoding.UTF8);
        var serializedData = await reader.ReadToEndAsync();

        // Assert
        serializedData.Should().Contain("0 Node 1");
        serializedData.Should().Contain("1 Node 2");
        serializedData.Should().Contain("0 -1");
        serializedData.Should().Contain("1 -1");
    }
    
    [TestMethod]
    public async Task Serialize_WithRandomLinks_ShouldWriteListNodesInStreamCorrectly()
    {
        // Arrange
        var serializer = ServiceProvider!.GetRequiredService<IListSerializer>();
        
        var node1 = new ListNode { Data = "Node 1" };
        var node2 = new ListNode { Data = "Node 2" };
        var node3 = new ListNode { Data = "Node 3" };

        node1.Next = node2;
        node1.Random = node3;
        node2.Next = node3;
        node2.Previous = node1;
        node3.Previous = node2;
        node3.Random = node1;

        // Act
        using var memoryStream = new MemoryStream();
        await serializer.Serialize(node1, memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(memoryStream, Encoding.UTF8);
        var serializedData = await reader.ReadToEndAsync();

        // Assert
        serializedData.Should().Contain("0 Node 1");
        serializedData.Should().Contain("1 Node 2");
        serializedData.Should().Contain("2 Node 3");
        serializedData.Should().Contain("0 2");
        serializedData.Should().Contain("1 -1");
        serializedData.Should().Contain("2 0");
    }

    [TestMethod]
    public async Task Deserialize_ShouldCreateListNodesFromStreamCorrectly()
    {
        // Arrange
        var serializer = ServiceProvider!.GetRequiredService<IListSerializer>();
        
        var serializedData = await GetSerializedData("0 Node 1\n1 Node 2\n", "0 -1\n1 -1\n");
        
        // Act
        using var memoryStream = new MemoryStream(serializedData);
        var resultNode1 = await serializer.Deserialize(memoryStream);
        var resultNode2 = resultNode1.Next;

        // Assert
        resultNode1.Should().NotBeNull();
        resultNode2.Should().NotBeNull();
        resultNode1.Data.Should().Be("Node 1");
        resultNode2.Data.Should().Be("Node 2");
        resultNode1.Previous.Should().BeNull();
        resultNode1.Next.Should().Be(resultNode2);
        resultNode1.Random.Should().BeNull();
        resultNode2.Previous.Should().Be(resultNode1);
        resultNode2.Next.Should().BeNull();
        resultNode2.Random.Should().BeNull();
    }
    
    [TestMethod]
    public async Task Deserialize_WithRandomLinks_ShouldCreateListNodesFromStreamCorrectly()
    {
        // Arrange
        var serializer = ServiceProvider!.GetRequiredService<IListSerializer>();
        
        var node1 = new ListNode { Data = "Node 1" };
        var node2 = new ListNode { Data = "Node 2" };
        var node3 = new ListNode { Data = "Node 3" };
        node1.Next = node2;
        node1.Random = node3;
        node2.Previous = node1;
        node2.Next = node3;
        node3.Previous = node2;
        node3.Random = node1;

        var serializedData = await GetSerializedData("0 Node 1\n1 Node 2\n2 Node 3\n", "0 2\n1 -1\n2 0\n");

        // Act
        using var memoryStream = new MemoryStream(serializedData);
        var resultHead = await serializer.Deserialize(memoryStream);

        // Assert
        CompareLinkedLists(node1, resultHead);
    }

    [TestMethod]
    public async Task DeepCopy_ShouldReturnExactCopy()
    {
        // Arrange
        var serializer = ServiceProvider!.GetRequiredService<IListSerializer>();
        
        var node1 = new ListNode { Data = "Node 1" };
        var node2 = new ListNode { Data = "Node 2" };

        node1.Next = node2;
        node2.Previous = node1;

        // Act
        var deepCopyHead = await serializer.DeepCopy(node1);

        // Assert
        deepCopyHead.Should().NotBeNull();
        deepCopyHead.Should().NotBeSameAs(node1);
        deepCopyHead.Data.Should().Be("Node 1");
        deepCopyHead.Next.Should().NotBeNull();
        deepCopyHead.Next.Should().NotBeSameAs(node2);
        deepCopyHead.Next.Data.Should().Be("Node 2");
        deepCopyHead.Next.Previous.Should().NotBeNull();
        deepCopyHead.Next.Previous.Should().NotBeSameAs(node1);
        deepCopyHead.Next.Previous.Data.Should().Be("Node 1");
        deepCopyHead.Next.Next.Should().BeNull();
    }

    [TestMethod]
    public async Task DeepCopy_WithRandomLinks_ShouldReturnExactCopy()
    {
        // Arrange
        var serializer = ServiceProvider!.GetRequiredService<IListSerializer>();
        
        var node1 = new ListNode { Data = "Node 1" };
        var node2 = new ListNode { Data = "Node 2" };
        var node3 = new ListNode { Data = "Node 3" };

        node1.Next = node2;
        node1.Random = node2;
        node2.Next = node3;
        node2.Previous = node1;
        node3.Random = node1;
        node3.Previous = node2;

        // Act
        var deepCopyHead = await serializer.DeepCopy(node1);

        // Assert
        CompareLinkedLists(node1, deepCopyHead);
    }

    private Task<byte[]> GetSerializedData(string data, string randomLinks)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);
        var dataBytesLength = BitConverter.GetBytes(dataBytes.Length);
        var randomLinksBytes = Encoding.UTF8.GetBytes(randomLinks);
        var randomLinksLength = BitConverter.GetBytes(randomLinksBytes.Length);
        
        var list = new List<byte>();
        list.AddRange(dataBytesLength);
        list.AddRange(dataBytes);
        list.AddRange(randomLinksLength);
        list.AddRange(randomLinksBytes);
        
        return Task.FromResult(list.ToArray());
    }

    private void CompareLinkedLists(ListNode original, ListNode copy)
    {
        while (original is not null && copy is not null)
        {
            original.Data.Should().Be(copy.Data);
            
            if (original.Previous is not null)
            {
                original.Previous.Data.Should().Be(copy.Previous.Data);
            }
            else
            {
                copy.Previous.Should().BeNull();
            }
            
            if (original.Random is not null)
            {
                original.Random.Data.Should().Be(copy.Random.Data);
            }
            else
            {
                copy.Random.Should().BeNull();
            }

            original = original.Next;
            copy = copy.Next;
        }

        original.Should().BeNull();
        copy.Should().BeNull();
    }
}