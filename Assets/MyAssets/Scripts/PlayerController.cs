using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Initialisation")]
    [SerializeField] float playerMass = 80f;
    [SerializeField] float collisionSweepDistance = 1f;
    [Header("Speed settings")]
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runMultiplier = 4;
    [SerializeField] float currentSpeed = 0;
    [SerializeField] float jumpMultiplier = 1f;
    [SerializeField] LayerMask groundMask;


    Vector3 moveDirection;
    bool isJump = false;
    bool isGrounded = true;

    Rigidbody rb => GetComponent<Rigidbody>();

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        GroundDetection();
    }

    void FixedUpdate()
    {
        //rb.Addforce(Vector3)  //houd rekening met velocity, mass en andere elementen
        //rb.Velocity = Vector3 //overschrijft alle elementen. Achteraf behoud de rb wel zijn massa en velocity
        //rb.MovePosition(rb.position*vector3); // Heeft dezelfde functie transform.Translate, maar in context van de physics step en rigidbody.

        rb.linearVelocity = CalculateMoveDirection();
        if(GroundDetection()&&isJump)
        {
            rb.AddForce(Vector3.up * jumpMultiplier , ForceMode.VelocityChange);
        }
    }

    void Initialize()
    {
        rb.mass = playerMass;
        currentSpeed = walkSpeed;
        isJump = false;
    }

    Vector3 CalculateMoveDirection()
    {
       
        Vector3 newDirection = moveDirection * currentSpeed;
        
        newDirection.y = rb.linearVelocity.y;
        

        return newDirection;
    }

    bool GroundDetection()
    {
        CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();
        float radius = playerCollider.radius;
        float distance = playerCollider.height * 0.5f;
        isGrounded = Physics.SphereCast(transform.position, radius, Vector3.down, out RaycastHit hit, distance, groundMask);
        print(isGrounded);
        return isGrounded;
    }
    
    #region Inputs
    public void OnMove(InputValue context)   //OnMove geeft aan "unused" omdat ons script het niet called unity zelf called het (geen echte fout)
    {
        Vector2 moveInput = context.Get<Vector2>();
        print("move value changed:" + moveInput);
        moveDirection = new Vector3(moveInput.x,0,moveInput.y);
    }
    
    public void OnJump(InputValue context)
    {
        isJump = context.isPressed;
    }
    #endregion
}
