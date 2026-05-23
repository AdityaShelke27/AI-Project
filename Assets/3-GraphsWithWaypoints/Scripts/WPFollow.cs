using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WPFollow : MonoBehaviour
{
    /*Transform goal;
    float speed = 5f;
    float accuracy = 1f;
    float rotSpeed = 2f;*/

    public WPManager WPManager;
    GameObject[] wps;
    GameObject currentNode;
    int currentWP = 0;
    NavMeshAgent agent;
    
    /*Graph g;
    List<Node> m_PathList;
    bool m_Done = false;*/

    void Start()
    {
        wps = WPManager.GetComponent<WPManager>().m_Waypoints;
        //g = WPManager.GetComponent<WPManager>().graph;
        currentNode = wps[0];
        agent = GetComponent<NavMeshAgent>();
    }

    public void GoToHel()
    {
        /*g.AStar(currentNode, wps[3]);
        currentWP = 0;
        m_PathList = g.GetPathList();
        m_Done = false;*/
        agent.SetDestination(wps[3].transform.position);
    }
    public void GoToRuins()
    {
        /*g.AStar(currentNode, wps[5]);
        currentWP = 0;
        m_PathList = g.GetPathList();
        m_Done = false;*/
        agent.SetDestination(wps[5].transform.position);
    }
    public void GoToLab()
    {
        /*g.AStar(currentNode, wps[9]);
        currentWP = 0;
        m_PathList = g.GetPathList();
        m_Done = false;*/
        agent.SetDestination(wps[9].transform.position);
    }
    void LateUpdate()
    {
        /*if (m_Done || m_PathList == null) return;

        if(Vector3.Distance(transform.position, currentNode.transform.position) <= accuracy)
        {
            currentWP++;
            if(currentWP >= m_PathList.Count)
            {
                m_Done = true;
                return;
            }
            currentNode = m_PathList[currentWP].GetID();
        }
        Quaternion targetRot = Quaternion.LookRotation(currentNode.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotSpeed);

        transform.Translate(speed * Time.deltaTime * Vector3.forward);*/
    }
}
