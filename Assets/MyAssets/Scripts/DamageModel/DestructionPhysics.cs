using UnityEngine;

public class DestructionPhysics : MonoBehaviour
{
    Rigidbody rb => GetComponent<Rigidbody>();
    [SerializeField] float force = 3000f;
    [SerializeField] float forceRadius = 0.3f;

    void Start()
    {
        rb.AddExplosionForce(force,transform.parent.position,forceRadius,1f,ForceMode.Impulse);
    }

   
}
