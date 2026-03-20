using UnityEngine;
using DG.Tweening;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] WeaponSO weapon;
    Vector3 startPosition;
    [SerializeField] float offset = 0.5f;
    [SerializeField] float animationTime = 1f;
    
    void Start()
    {
        startPosition = transform.position;
        transform.DOMoveY(startPosition.y + offset, animationTime)
        .SetLoops(-1, LoopType.Yoyo);
        transform.DORotate(new Vector3(0f, 360f, 0f), animationTime, RotateMode.LocalAxisAdd)
        .SetLoops(-1)
        .SetEase(Ease.Linear);

    }

    void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        FakeInventory.instance.PickUpItem(weapon);
        Destroy(gameObject);
    }

}
