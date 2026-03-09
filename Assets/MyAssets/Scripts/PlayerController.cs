using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Initialisation")]
    [SerializeField] float playerMass = 80f;
    [SerializeField] float collisionSweepDistance = 1f;
    enum CameraMode { firstPerson, thirdPerson }
    [SerializeField] CameraMode cameraMode = CameraMode.firstPerson;
    Camera mainCam => Camera.main;
    [Header("Move settings")]
    Vector3 moveDirection;
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runMultiplier = 4;
    [SerializeField] float currentSpeed = 0;
    [SerializeField] float jumpMultiplier = 1f;
    [SerializeField] int maxJumpCount = 1;
    int jumpCount = 0;
    bool isMoving = false;

    [Header("Ground and Slope Detection")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] float maxSlopeAngle;
    bool isJump = false;
    bool isGrounded = true;

    [Header("Stairs Detection")]
    [SerializeField] float stepHeight = 0.5f;

    Rigidbody rb => GetComponent<Rigidbody>();

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        UpdateRotation();
    }

    void UpdateRotation()
    {
        switch (cameraMode)
        {
            case CameraMode.firstPerson:
                Vector3 camForward = mainCam.transform.forward;
                camForward.y = 0; // we zetten de y component van onze camForward op 0 zodat we alleen in het horizontale vlak kijken. zo voorkomen we dat we omhoog of omlaag kijken als we onze forward richting updaten.
                transform.forward = camForward.normalized; // we zetten onze forward richting gelijk aan onze camera zodat we altijd in de richting van onze camera kijken.
                break;
            case CameraMode.thirdPerson:

                if (isMoving) transform.forward = moveDirection.normalized; // we zetten onze forward richting gelijk aan onze movedirection zodat we altijd in de richting van onze movement kijken.
                break;
        }
    }
    void Initialize()
    {
        rb.mass = playerMass;
        currentSpeed = walkSpeed;
        isJump = false;
    }

    void FixedUpdate()
    {
        Movement();
        jump();
    }
    void Movement()
    {
        //rb.Addforce(Vector3)  //houd rekening met velocity, mass en andere elementen
        //rb.Velocity = Vector3 //overschrijft alle elementen. Achteraf behoud de rb wel zijn massa en velocity
        //rb.MovePosition(rb.position*vector3); // Heeft dezelfde functie transform.Translate, maar in context van de physics step en rigidbody.

        Vector3 velocity = rb.linearVelocity;
        RaycastHit hit;
        isGrounded = GroundDetection(out hit);

        //print(hit.collider?.name); // hetzelfde als "if(hit.collider != null) print(hit.collider.name);"
        float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

        //DetectStairs();

        if (isGrounded)
        {
            if (slopeAngle <= maxSlopeAngle)
            {
                //copy onze movedirection naar moveDir
                Vector3 moveDir = CalculateMoveDirection();
                //we projecteren moveDir zodat hij parallel loopt met de slope waar we op zitten. zo behouden we onze velocity.
                moveDir = Vector3.ProjectOnPlane(moveDir, hit.normal);
                velocity.x = moveDir.x;
                velocity.z = moveDir.z;
            }
            else
            {
                Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal);
                velocity += slopeDirection * currentSpeed;
            }
        }


        rb.linearVelocity = velocity;

    }

    void jump()
    {
        if (isGrounded && isJump && jumpCount < maxJumpCount)
        {
            rb.AddForce(Vector3.up * jumpMultiplier, ForceMode.VelocityChange);
            jumpCount++;
        }

    }

    /*void DetectStairs()
    {
        RaycastHit hit;
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        
        float radius = capsuleCollider.radius;

        if(Physics.CapsuleCast(point1, point2, radius, transform.forward, out hit, 2f, groundMask))
        {
            Vector3 downOrigin = hit.point * stepHeight;
            if(Physics.Raycast(downOrigin, Vector3.down, out RaycastHit hit2, stepHeight, groundMask))
            {
                float height = hit2.point.y - point2.y;
                if(height > 0f && height < stepHeight)
                {
                    rb.position += Vector3.up * height;
                }
            }
        }
    }*/
    Vector3 TransformToCameraSpace(Vector3 input)
    {
        float splitX, splitZ;
        splitX = input.x;
        splitZ = input.z;
        Vector3 camX, camZ;
        camX = mainCam.transform.right * splitX;
        camZ = mainCam.transform.forward * splitZ;
        return camX + camZ;

    }
    #region Inputs
    Vector2 moveInput;
    public void OnMove(InputValue context)   //OnMove geeft aan "unused" omdat ons script het niet called unity zelf called het (geen echte fout)
    {
         moveInput = context.Get<Vector2>();
        //print("move value changed:" + moveInput);
     
        isMoving = moveInput.x != 0 || moveInput.y != 0; // we checken of er input is, zo niet zetten we isMoving op false zodat we niet onnodig onze forward richting updaten. 
        //transform.forward = moveDirection.normalized; // we zetten onze forward richting gelijk aan onze movedirection zodat we altijd in de richting van onze movement kijken.
    }

    
    public void OnJump(InputValue context)
    {
        isJump = context.isPressed;
    }
    #endregion

    Vector3 CalculateMoveDirection()
    {
        moveDirection = TransformToCameraSpace(moveInput);
        Vector3 newDirection = moveDirection * currentSpeed;

        newDirection.y = rb.linearVelocity.y;


        return newDirection;
    }
    

    bool GroundDetection(out RaycastHit hit)
    {
        CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();
        float radius = playerCollider.radius;
        float distance = playerCollider.height * 0.5f;

        

        if(Physics.SphereCast(transform.position, radius, Vector3.down, out hit, distance, groundMask))
        {
            jumpCount = 0;
            return true;
        }

        return false;
    }
    
}
