using UnityEngine;

public class Edge
{
    public Node startNode;
    public Node endNode;

    public Edge(Node start, Node end)
    {
        startNode = start;
        endNode = end;
    }
}
