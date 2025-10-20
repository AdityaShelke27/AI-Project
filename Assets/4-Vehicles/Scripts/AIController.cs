using UnityEngine;

public class AIController : MonoBehaviour
{
    Drive[] ds;
    [SerializeField] Circuit Circuit;
    [SerializeField] float steeringSensitivity = 0.01f;
    [SerializeField] GameObject breakLight;
    Rigidbody rb;
    Vector3 target;
    int currentWP = 0;
    void Start()
    {
        ds = GetComponentsInChildren<Drive>();
        target = Circuit.m_WayPoints[currentWP].position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 localTarget = transform.InverseTransformPoint(target);
        float distance = Vector3.Distance(transform.position, target);
        float angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        float acc = 1f;
        float steer = Mathf.Clamp(angle * steeringSensitivity, -1, 1);
        float brake = 0;
        if(distance < 10)
        {
            brake = 0.7f;
        }
        for(int i = 0; i < ds.Length; i++)
        {
            ds[i].Go(acc, steer, brake);
        }
        if(brake > 0)
        {
            breakLight.SetActive(true);
        }
        else
        {
            breakLight.SetActive(false);
        }
        if (distance <= 4)
        {
            currentWP = (currentWP + 1) % Circuit.m_WayPoints.Length;
            target = Circuit.m_WayPoints[currentWP].position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.InverseTransformPoint(target));
    }
}
