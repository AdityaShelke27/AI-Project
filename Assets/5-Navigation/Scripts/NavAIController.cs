using UnityEngine;
using UnityEngine.AI;

public class NavAIController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform m_Target;
    Animator animator;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        agent.SetDestination(m_Target.position);
        if(agent.velocity.magnitude > 0f)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }
}
