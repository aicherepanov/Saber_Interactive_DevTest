using System;
using SerializerTests.Nodes;

namespace Services.UnitTests;

public static class TestDataGenerator
{
    private static readonly Random _random = new();
    
    public static ListNode GenerateRandomLinkedList(int size)
    {
        var nodes = new ListNode[size];

        for (var i = 0; i < size; i++)
        {
            nodes[i] = new ListNode
            {
                Data = $"Node {i}",
                Previous = i > 0 ? nodes[i - 1] : null
            };
        }

        for (var i = 0; i < size; i++)
        {
            nodes[i].Next = i < size - 1 ? nodes[i + 1] : null;
            nodes[i].Random = GetRandomNode(nodes, size, i);
        }

        return nodes[0];
    }

    private static ListNode GetRandomNode(ListNode[] nodes, int size, int currentIndex)
    {
        if (size == 0)
        {
            return null;
        }

        int randomIndex;
        do
        {
            randomIndex = _random.Next(size);
        } while (randomIndex == currentIndex);
        
        return nodes[randomIndex];
    }
}