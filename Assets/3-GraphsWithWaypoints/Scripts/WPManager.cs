using System;
using UnityEngine;

[Serializable]
public struct Link
{
    public enum direction { UNI, BI };
    public GameObject node1;
    public GameObject node2;
    public direction dir;
}
public class WPManager : MonoBehaviour
{
    public GameObject[] m_Waypoints;
    public Link[] m_Links;
    public Graph graph = new Graph();
    void Start()
    {
        for(int i = 0; i < m_Waypoints.Length; i++)
        {
            graph.AddNode(m_Waypoints[i]);
        }
        for (int i = 0; i < m_Links.Length; i++)
        {
            graph.AddEdge(m_Links[i].node1, m_Links[i].node2);

            if (m_Links[i].dir == Link.direction.BI)
            {
                graph.AddEdge(m_Links[i].node2, m_Links[i].node1);
            }
        }
    }

    void Update()
    {
        
    }
}
