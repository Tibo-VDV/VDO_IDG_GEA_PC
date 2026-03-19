using UnityEngine;

public class RemoveProjectile : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject);    
    }
}
