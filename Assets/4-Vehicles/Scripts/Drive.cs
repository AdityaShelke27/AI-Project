using UnityEngine;

public class Drive : MonoBehaviour
{
    WheelCollider WC;
    [SerializeField] Transform m_WheelObject;
    [SerializeField] bool CanTurn;
    public float Torque = 400;
    public float MaxSteerAngle = 60;
    public float MaxBreakingTorque = 500;

    private void Start()
    {
        WC = GetComponent<WheelCollider>();
    }

    public void Go(float acc, float steer, float brake)
    {
        acc = Mathf.Clamp(acc, -1, 1);
        brake = Mathf.Clamp(brake, -1, 1);

        if (CanTurn) 
        {
            steer = Mathf.Clamp(steer, -1, 1);
            WC.steerAngle = steer * MaxSteerAngle;
        }

        WC.brakeTorque = brake * MaxBreakingTorque;
        WC.motorTorque = acc * Torque;
        WC.GetWorldPose(out Vector3 pos, out Quaternion quat);
        m_WheelObject.position = pos;
        m_WheelObject.rotation = quat;
    }
}
