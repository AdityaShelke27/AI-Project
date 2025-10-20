using UnityEngine;

public class Drive : MonoBehaviour
{
    WheelCollider WC;
    [SerializeField] Transform m_WheelObject;
    [SerializeField] float Torque = 400;
    [SerializeField] float MaxSteerAngle = 60;
    [SerializeField] float MaxBreakingTorque = 500;

    private void Start()
    {
        WC = GetComponent<WheelCollider>();
    }
    /*void Update()
    {
        float acc = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");
        Go(acc, steer);
    }*/

    public void Go(float acc, float steer, float brake)
    {
        acc = Mathf.Clamp(acc, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1);
        brake = Mathf.Clamp(brake, -1, 1);
        WC.brakeTorque = brake * MaxBreakingTorque;
        WC.motorTorque = acc * Torque;
        WC.steerAngle = steer * MaxSteerAngle;
        WC.GetWorldPose(out Vector3 pos, out Quaternion quat);
        m_WheelObject.position = pos;
        m_WheelObject.rotation = quat;
    }
}
