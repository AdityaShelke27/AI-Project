using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Edge> edgeList = new();
    public Node path = null;
    GameObject id;
    public Vector3 pos;

    public float f, g, h;
    public Node cameFrom;

    public Node(GameObject i)
    {
        id = i;
        pos = i.transform.position;
        path = null;
    }

    public GameObject GetID()
    {
        return id;
    }
}
