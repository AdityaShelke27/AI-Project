using UnityEngine;

public class FollowWP : MonoBehaviour
{
    [SerializeField] float m_Speed;
    [SerializeField] float m_RotSpeed = 5;
    [SerializeField] float lookAhead = 10;
    [SerializeField] Transform[] m_WayPoints;
    int m_CurrentWP = 0;
    GameObject tracker;

    private void Start()
    {
        tracker = new GameObject(gameObject.name + " tracker");

        tracker.transform.position = transform.position + transform.forward * 2;
        tracker.transform.rotation = transform.rotation;
    }
    void ProgressTracker()
    {
        if (Vector3.Distance(tracker.transform.position, transform.position) > lookAhead) return;

        if (Vector3.Distance(tracker.transform.position, m_WayPoints[m_CurrentWP].position) < 3)
        {
            m_CurrentWP = (m_CurrentWP + 1) % m_WayPoints.Length;
        }
        tracker.transform.LookAt(m_WayPoints[m_CurrentWP].position);
        tracker.transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
    }
    void Update()
    {
        ProgressTracker();

        Quaternion targetRot = Quaternion.LookRotation(tracker.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, m_RotSpeed * Time.deltaTime);
        transform.Translate(transform.forward * m_Speed * Time.deltaTime, Space.World);
    }
}
