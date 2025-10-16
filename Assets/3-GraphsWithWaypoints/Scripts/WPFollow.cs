using UnityEngine;

public class WPFollow : MonoBehaviour
{
    Transform goal;
    float speed = 5f;
    float accuracy = 1f;
    float rotSpeed = 2f;

    public WPManager WPManager;
    GameObject[] wps;
    GameObject currentNode;
    int currentWP = 0;
    Graph g;

    void Start()
    {
        wps = WPManager.GetComponent<WPManager>().m_Waypoints;
        g = WPManager.GetComponent<WPManager>().graph;
        currentNode = wps[0];
    }

    void GoToHel()
    {
        g.AStar(currentNode, wps[3]);
        currentWP = 0;
    }
    void GoToRuins()
    {
        g.AStar(currentNode, wps[5]);
        currentWP = 0;
    }
    void GoToLab()
    {
        g.AStar(currentNode, wps[9]);
        currentWP = 0;
    }
    void Update()
    {
        
    }
}
