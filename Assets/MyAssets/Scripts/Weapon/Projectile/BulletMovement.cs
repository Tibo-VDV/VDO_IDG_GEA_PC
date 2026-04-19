using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    
    Rigidbody rb => GetComponent<Rigidbody>();
    float velocity;

    void Start()
    {  
        rb.AddForce(transform.forward*velocity, ForceMode.VelocityChange);
    }

   public void SetVelocity(float vel)
    {
        velocity = vel;
    }

    
}
