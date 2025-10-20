using UnityEngine;

public class Flip : MonoBehaviour
{
    Rigidbody rb;
    float lastTimeChecked;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void RightCar()
    {
        transform.position += Vector3.up;
        transform.rotation = Quaternion.LookRotation(transform.forward);
    }
    void Update()
    {
        if(transform.up.y > 0.5 || rb.linearVelocity.magnitude > 1)
        {
            lastTimeChecked = Time.time;
        }

        if(Time.time > lastTimeChecked + 3)
        {
            RightCar();
        }
    }
}
