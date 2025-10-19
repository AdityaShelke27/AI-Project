using UnityEngine;

public class Drive : MonoBehaviour
{
    WheelCollider WC;
    [SerializeField] Transform m_WheelObject;
    [SerializeField] float Torque = 400;
    [SerializeField] float MaxSteerAngle = 30;

    private void Start()
    {
        WC = GetComponent<WheelCollider>();
    }
    void Update()
    {
        float acc = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");
        Go(acc, steer);
    }

    void Go(float acc, float steer)
    {
        acc = Mathf.Clamp(acc, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1);
        WC.motorTorque = acc * Torque;
        WC.steerAngle = steer * MaxSteerAngle;
        WC.GetWorldPose(out Vector3 pos, out Quaternion quat);
        m_WheelObject.position = pos;
        m_WheelObject.rotation = quat;
    }
}
