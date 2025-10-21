using UnityEngine;

public class AvoidDetector : MonoBehaviour
{
    public float avoidPath = 0;
    public float avoidTime = 0;
    public float wanderDistance = 4;
    public float avoidLength = 1;
    public bool reverse = false;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerExit(Collider other)
    {
        reverse = false;
        if (!other.CompareTag("car")) return;

        avoidTime = 0;
    }
    private void OnTriggerStay(Collider other)
    {
        Vector3 collisionDir = transform.InverseTransformPoint(other.transform.position);
        if(collisionDir.x > 0 && collisionDir.z > 0)
        {
            if(rb.linearVelocity.magnitude < 1) reverse = true;

            if (other.CompareTag("car"))
            {
                Rigidbody rb = other.GetComponent<Rigidbody>();
                avoidTime = Time.time + avoidLength;

                Vector3 otherCarLocalTarget = transform.InverseTransformPoint(rb.position);
                float otherCarAngle = Mathf.Atan2(otherCarLocalTarget.x, otherCarLocalTarget.z);
                avoidPath = wanderDistance * -Mathf.Sign(otherCarAngle);
            }
        }
    }
}
