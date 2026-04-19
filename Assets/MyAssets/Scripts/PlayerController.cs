using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Initialisation")]
    [SerializeField] float playerMass = 80f;
    [SerializeField] float collisionSweepDistance = 1f;
    public enum MoveMode { firstPersonMove, thirdPersonMove }
    [SerializeField] MoveMode moveMode = MoveMode.firstPersonMove;
    Camera mainCam => Camera.main;
    [Header("Move settings")]
    Vector3 moveDirection;
    Vector3 camDir;
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runMultiplier = 4;
    public float currentSpeed { get; private set; } //{ get; private set; }, zorgt ervoor dat onze public variable read only is.
    //Dit omdat ze specifiek vermeld dat de "set" private is
    //Zo voorkomen we dat externe code deze waarde per ongeluk overschrijft
    [SerializeField] float jumpMultiplier = 1f;
    [SerializeField] int maxJumpCount = 1;
    int jumpCount = 0;
    public bool isMoving { get; private set; }

    [Header("Ground and Slope Detection")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] float maxSlopeAngle;
    bool isJump = false;
    bool isGrounded = true;

    [Header("Stairs Detection")]
    [SerializeField] float stepHeight = 0.5f;

    [Header("Weapon item reference")]
    [SerializeField] WeaponItem weaponItem;
    [SerializeField] WeaponAnimationController getWeaponAnimationController;

    public bool playWalkingSound = false;

    public Rigidbody rb => GetComponent<Rigidbody>();

    public static PlayerController instance;

    public event Action walking;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        
    }

    void Start()
    {
        Initialize();
    }

    void LateUpdate()
    {
        
        UpdateRotation(moveDirection);
    }

    void UpdateRotation(Vector3 rotateDirection)
    {
        switch (moveMode)
        {
            case MoveMode.firstPersonMove:
                Vector3 camDir = mainCam.transform.forward;
                camDir.y = 0; // we zetten de y component van onze camForward op 0 zodat we alleen in het horizontale vlak kijken. zo voorkomen we dat we omhoog of omlaag kijken als we onze forward richting updaten.
                transform.forward = camDir.normalized; // we zetten onze forward richting gelijk aan onze camera zodat we altijd in de richting van onze camera kijken.
                break;
            case MoveMode.thirdPersonMove:
                if (isMoving)
                    transform.forward = rotateDirection.normalized; // we zetten onze forward richting gelijk aan onze movedirection zodat we altijd in de richting van onze movement kijken.
                break;
        }
    }
    void Initialize()
    {
        rb.mass = playerMass;
        currentSpeed = 0;
        isJump = false;
        isSprinting = false;
    }

    void FixedUpdate()
    {
        Movement();
        jump();
        UpdateJumpAnimationState();
       
    }

    public void UpdateWeapon(WeaponItem weapon)
    {
        weaponItem = weapon;
        getWeaponAnimationController = weaponItem.weaponGameObject.GetComponent<WeaponAnimationController>();
    }
    
    void Movement()
    {
        //rb.Addforce(Vector3)  //houd rekening met velocity, mass en andere elementen
        //rb.Velocity = Vector3 //overschrijft alle elementen. Achteraf behoud de rb wel zijn massa en velocity
        //rb.MovePosition(rb.position*vector3); // Heeft dezelfde functie transform.Translate, maar in context van de physics step en rigidbody.

        currentSpeed = isSprinting ? walkSpeed * runMultiplier : walkSpeed; // isSprinting? is bool true voor : false erna. 
        currentSpeed = isMoving ? currentSpeed : 0f;

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
                //print(moveDir);
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
    void UpdateJumpAnimationState()
    {
        if(getWeaponAnimationController == null) return;
        getWeaponAnimationController.SetJumpState(isGrounded);
        getWeaponAnimationController.SetJumpDirection(rb.linearVelocity.y);
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

        Vector3 camX, camZ;
        camX = mainCam.transform.right * input.x;
        camZ = mainCam.transform.forward * input.y;
        Vector3 finalDirection = camX + camZ;
        finalDirection.y = 0;
        return finalDirection;

    }
    
    public void SetMoveMode(MoveMode setMode)
    {
        moveMode = setMode;
    }
    #region Inputs
    Vector2 moveInput;
    public void OnMove(InputValue context)   //OnMove geeft aan "unused" omdat ons script het niet called unity zelf called het (geen echte fout)
    {
        moveInput = context.Get<Vector2>();
        //print("move value changed:" + moveInput);

        isMoving = moveInput.x != 0 || moveInput.y != 0; // we checken of er input is, zo niet zetten we isMoving op false zodat we niet onnodig onze forward richting updaten. 
        //transform.forward = moveDirection.normalized; // we zetten onze forward richting gelijk aan onze movedirection zodat we altijd in de richting van onze movement kijken.

        if(!playWalkingSound)
            {
            walking.Invoke();
            playWalkingSound = true;
                
            }
        
    }
    
    public bool isSprinting = false;

    public void OnSprint(InputValue context)
    {
        isSprinting = context.isPressed;
        
       //print("sprint pressed");
    }

    
    public void OnJump(InputValue context)
    {
        isJump = context.isPressed;
    }
    #endregion

    Vector3 CalculateMoveDirection()
    {

        moveDirection = TransformToCameraSpace(moveInput);
        //print("move direction:" + moveDirection);
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
