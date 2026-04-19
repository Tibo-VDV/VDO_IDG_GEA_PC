using System.Collections;
using UnityEngine;

public class PlayerSoundControlls : MonoBehaviour
{
        AudioSource AudioSource => GetComponent<AudioSource>();
        [SerializeField] AudioClip walking;
        [SerializeField] AudioClip running;

    void OnEnable()
    {
        PlayerController.instance.walking += PlayWalkingSound;
        
        
    }
    
    IEnumerator walkingSound()
    {
        while (true)
        {
            print("start walking sound");
            AudioClip tmpClip =ChoseSound();
            if(tmpClip == null)
            {
                PlayerController.instance.playWalkingSound = false;
                yield return null;
                yield break;  
            } 

            AudioSource.PlayOneShot(tmpClip);
            yield return new WaitForSeconds(tmpClip.length);

        }

    }
    AudioClip ChoseSound()
    {
        if(PlayerController.instance.isMoving)
        {
            
            if(PlayerController.instance.isSprinting)
            {
                print("running");
                return running;
            }
            else 
            {
                print("walking");
                return walking;
            }
        }
        else return null;
    }

    void PlayWalkingSound()
    {    

       StartCoroutine(walkingSound());
        
    }
}
