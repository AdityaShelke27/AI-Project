using UnityEngine;

public class AIController : MonoBehaviour
{
    Drive[] ds;
    [SerializeField] Circuit Circuit;
    [SerializeField] GameObject breakLight;
    [Header("Car Settings")]
    [SerializeField] float steeringSensitivity = 0.01f;
    [SerializeField] float Torque = 400;
    [SerializeField] float MaxSteerAngle = 60;
    [SerializeField] float MaxBreakingTorque = 500;
    [SerializeField] float antiRoll = 2000;
    [SerializeField] float maxCornerAngle = 90;
    [SerializeField] float cornerThreshold = 20;
    [SerializeField] float vehicleSpeedThreshold = 10;
    Rigidbody rb;
    Vector3 target;
    int currentWP = 0;
    GameObject tracker;
    AvoidDetector avoid;
    int currentTrackerWP = 0;
    float trackerLookAhead = 30;
    void Start()
    {
        ds = GetComponentsInChildren<Drive>();
        target = Circuit.m_WayPoints[currentWP].position;
        rb = GetComponent<Rigidbody>();

        tracker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        Destroy(tracker.GetComponent<Collider>());
        tracker.transform.position = transform.position;
        tracker.transform.rotation = transform.rotation;

        avoid = GetComponent<AvoidDetector>();

        foreach (Drive d in ds)
        {
            d.Torque = Torque;
            d.MaxSteerAngle = MaxSteerAngle;
            d.MaxBreakingTorque = MaxBreakingTorque;
        }
    }
    void ProgressTracker()
    {
        //Debug.DrawLine(transform.position, tracker.transform.position);
        if(Vector3.Distance(transform.position, tracker.transform.position) > trackerLookAhead) return;

        tracker.transform.LookAt(Circuit.m_WayPoints[currentTrackerWP]);
        tracker.transform.Translate(Vector3.forward * (rb.linearVelocity.magnitude * 2) * Time.deltaTime);

        if(Vector3.Distance(tracker.transform.position, Circuit.m_WayPoints[currentTrackerWP].position) < 1)
        {
            currentTrackerWP = (currentTrackerWP + 1) % Circuit.m_WayPoints.Length;
        }
    }
    void Update()
    {
        ProgressTracker();
        target = tracker.transform.position;
        Vector3 localTarget;

        if(Time.time < avoid.avoidTime)
        {
            localTarget = tracker.transform.right * avoid.avoidPath;
        }
        else
        {
            localTarget = transform.InverseTransformPoint(target);
        }
        Debug.DrawLine(localTarget - Vector3.one, localTarget + Vector3.one, Color.red);
        float angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        float steer = Mathf.Clamp(angle * steeringSensitivity, -1, 1);

        float corner = Mathf.Clamp(angle, 0, maxCornerAngle);
        float cornerNorm = corner / maxCornerAngle;

        float acc = 1f;
        float vehicleSpeed = rb.linearVelocity.magnitude;
        if (corner > cornerThreshold && vehicleSpeed > vehicleSpeedThreshold)
        {
            acc = Mathf.Lerp(0, 1, 1 - cornerNorm);
        }

        float brake = 0;
        if (corner > cornerThreshold && vehicleSpeed > vehicleSpeedThreshold)
        {
            brake = Mathf.Lerp(0, 1, cornerNorm);
        }
        if(avoid.reverse)
        {
            acc *= -1;
            steer *= -1;
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
    }

    public float GetAntiRoll()
    {
        return antiRoll;
    }
}
