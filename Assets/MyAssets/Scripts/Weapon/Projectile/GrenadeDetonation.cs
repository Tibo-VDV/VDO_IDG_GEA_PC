using System.Collections;
using DG.Tweening;
using UnityEngine;


public class GrenadeDetonation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject explosionSphere;
    [SerializeField] float explosionDuration = 1f;
    [SerializeField] float explosionSize = 3f;
     Rigidbody rb => GetComponent<Rigidbody>();
    [SerializeField] float force = 3000f;
    [SerializeField] float forceRadius = 0.3f;
    void Start()
    {
        StartCoroutine(Detonation());
    }

    IEnumerator Detonation()
    {
        print("Start timer");
        yield return new WaitForSeconds(3f);
        //rb.AddExplosionForce(force,transform.parent.position,forceRadius,1f,ForceMode.Impulse);
        explosionSphere.SetActive(true);
        explosionSphere.transform.DOScale(explosionSize,explosionDuration);
        yield return new WaitForSeconds(explosionDuration);
        Destroy(gameObject);
        print ("Boom");
    }
}
