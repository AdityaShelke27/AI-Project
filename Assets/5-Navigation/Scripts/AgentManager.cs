using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentManager : MonoBehaviour
{
    List<NavMeshAgent> agents = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("AI");

        foreach (GameObject obj in objs)
        {
            agents.Add(obj.GetComponent<NavAIController>().agent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                foreach (var agent in agents)
                {
                    agent.SetDestination(hit.point);
                }
            }
        }
    }
}
