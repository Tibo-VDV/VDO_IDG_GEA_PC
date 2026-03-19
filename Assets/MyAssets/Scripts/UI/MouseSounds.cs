using UnityEngine;
using UnityEngine.EventSystems;


public class MouseSounds : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip selectSound;
    [SerializeField] AudioClip[] hoverSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundSource.playOnAwake = false;
        soundSource.loop = false;
    }

   public void OnPointerEnter(PointerEventData eventData)
    {
        soundSource.PlayOneShot(hoverSound[Random.Range(0,hoverSound.Length)]);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        soundSource.PlayOneShot(selectSound);
        
    }
}
