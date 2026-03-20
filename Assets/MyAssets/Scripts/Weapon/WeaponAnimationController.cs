using UnityEngine;

public class WeaponAnimationController : MonoBehaviour
{
    Animator animator => GetComponent<Animator>();
    [SerializeField] PlayerController getPlayerController;
    bool initialized = false;

    void Start()
    {
        if (getPlayerController == null)
        {
            Debug.LogError("PlayerController reference is missing, assign reference.");
            return;
        }
        if (animator == null)
        {
            Debug.LogError("Animator component is missing, add an Animator component.");
            return;
        }
        initialized = true;
    }

    public void SetMoveSpeed(float speed)
    {
        if (!initialized) return;
        animator.SetFloat("_moveSpeed", speed);
        
    }

    public void FireWeapon(int InputValue)
    {
        bool fireGun = IntBasedBool(InputValue);
        animator.SetBool("_fire", fireGun);
    }
    public bool GetFireState()
    {
        return animator.GetBool("_fire");
    }

    public void SetJumpState(bool state)
    {
        animator.SetBool("_isGrounded", state);
    }

    bool IntBasedBool(int _value)
    {
        return _value == 1 ? true : false;
    }

    public void SetJumpDirection(float value)
    {
        animator.SetFloat("_jumpDirection",value);
    }
}
