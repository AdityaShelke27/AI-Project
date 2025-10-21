using UnityEngine;

public class AntiRoll : MonoBehaviour
{
    float antiRoll = 5000;
    [SerializeField] WheelCollider wheelLFront;
    [SerializeField] WheelCollider wheelRFront;
    [SerializeField] WheelCollider wheelLBack;
    [SerializeField] WheelCollider wheelRBack;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        antiRoll = GetComponent<AIController>().GetAntiRoll();
    }

    void Update()
    {
        GroundWheels(wheelLFront, wheelRFront);
        GroundWheels(wheelLBack, wheelRBack);
    }

    void GroundWheels(WheelCollider WL, WheelCollider WR)
    {
        WheelHit hit;
        float travelL = 1;
        float travelR = 1;

        bool groundedL = WL.GetGroundHit(out hit);
        if(groundedL)
        {
            travelL = -(WL.transform.InverseTransformPoint(hit.point).y - WL.radius) / WL.suspensionDistance;
        }
        bool groundedR = WR.GetGroundHit(out hit);
        if (groundedR)
        {
            travelR = -(WR.transform.InverseTransformPoint(hit.point).y - WR.radius) / WR.suspensionDistance;
        }

        float antiRollForce = (travelR - travelL) * antiRoll;
        if(groundedL)
        {
            rb.AddForceAtPosition(WL.transform.up * -antiRollForce, WL.transform.position);
        }
        if(groundedR)
        {
            rb.AddForceAtPosition(WR.transform.up * -antiRollForce, WR.transform.position);
        }
    }
}
