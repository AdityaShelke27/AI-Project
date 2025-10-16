using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    List<Edge> edgeList = new();
    List<Node> nodeList = new();
    List<Node> pathList = new();

    public void AddNode(GameObject node)
    {
        nodeList.Add(new(node));
    }
    public void AddEdge(GameObject fromNode, GameObject toNode)
    {
        Node from = FindNode(fromNode);
        Node to = FindNode(toNode);

        if(from != null && to != null)
        {
            Edge edge = new(from, to);

            edgeList.Add(edge);
            from.edgeList.Add(edge);
        }
    }
    public bool AStar(GameObject startID, GameObject endID)
    {
        Node start = FindNode(startID);
        Node end = FindNode(endID);

        if(start == null || end == null)
        {
            return false;
        }

        List<Node> open = new();
        List<Node> closed = new();
        float tentative_g_score = 0;
        bool tentative_is_better;

        start.g = 0;
        start.h = Distance(start, end);
        start.f = start.h;

        open.Add(start);

        while(open.Count > 0)
        {
            int i = LowestF(open);
            Node thisNode = open[i];

            if(thisNode == end)
            {
                // ReconstructPath(start, end);
                return true;
            }

            open.RemoveAt(i);
            closed.Add(thisNode);
            Node neighbor;

            foreach(Edge e in thisNode.edgeList)
            {
                neighbor = e.endNode;

                if(closed.IndexOf(neighbor) > -1)
                {
                    continue;
                }

                tentative_g_score = thisNode.g + Distance(thisNode, neighbor);

                if(open.IndexOf(neighbor) == -1)
                {
                    open.Add(neighbor);
                    tentative_is_better = true;
                }
                else if(tentative_g_score < neighbor.g)
                {
                    tentative_is_better = true;
                }
                else
                {
                    tentative_is_better = false;
                }

                if(tentative_is_better)
                {
                    neighbor.cameFrom = thisNode;
                    neighbor.g = tentative_g_score;
                    neighbor.h = Distance(neighbor, end);
                    neighbor.f = neighbor.g + neighbor.h;
                }
            }
        }

        return false;
    }
    public void Reconstructpath(Node start, Node end)
    {
        pathList.Clear();
        pathList.Add(end);

        Node p = end.cameFrom;
        while(p != start && p != null)
        {
            pathList.Insert(0, p);
            p = p.cameFrom;
        }

        pathList.Insert(0, start);
    }
    public Node FindNode(GameObject node)
    {
        return nodeList.Find(p => p.GetID() == node);
    }

    float Distance(Node a, Node b)
    {
        return Vector3.SqrMagnitude(a.GetID().transform.position - b.GetID().transform.position);
    }
    int LowestF(List<Node> l)
    {
        int idx = 0;
        Node lowF = l[0];
        for(int i = 0; i < l.Count; i++)
        {
            if(lowF.f > l[i].f)
            {
                lowF = l[i];
                idx = i;
            }
        }

        return idx;
    }
}
