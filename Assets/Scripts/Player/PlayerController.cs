//by TheSuspect
//19.10.2023

using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PlayerController : Damageable
{

    [Header("References")]
    public Transform orientation;
    [HideInInspector] public Rigidbody rb;
    public PlayerCam cam;
    public ParticleSystem sprintParticles;

    [Space]

    [Header("Movement")]

    public float currentSpeed;
    public float walkSpeed = 7f;
    public float sprintSpeed = 12f;

    [Space]

    private Vector3 moveDir;

    [Space]

    public float maxSlopeAngle = 40f;
    private RaycastHit slopeHit;
    private bool extitingSlope;


    [Header("Jumping")]

    public float airMultiplayer = 1f;
    public float groundDrag = 5f;
    public float jumpForce = 6f;
    public float jumpCooldwn = 0.75f;
    bool readyToJump = true;

    [Space]

    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    public bool grounded;


    [Header("Statements")]

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        air
    }

    public bool isSprinting;


    [Header("Input")]

    public InputManager input;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;


    private void Awake()
    {
        SetUp();
    }

    private void Start() {



    }

    private void Update()
    {
        Debug();

        StateHandler();
        MyInput();
        SpeedControl();
        GroundCheck();

        //Anti Falloff
        if(transform.position.y <-25f) {TakeDamage(currentHealth/3);transform.position=new Vector3(transform.position.x,15,transform.position.z);rb.velocity=Vector3.zero;}


        if(isSprinting && moveDir!=Vector3.zero) sprintParticles.gameObject.SetActive(true);
        else sprintParticles.gameObject.SetActive(false);

    }

    private void FixedUpdate()
    {

        Movement();

    }

    //--------------------------------------------------------------------------------------------------------------------------------

    private void SetUp()
    {
        
        rb = GetComponent<Rigidbody>();

        currentHealth = maxHealth;
        
    }

    //--------------------------------------------------------------------------------------------------------------------------------

    #region Movement
    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f, whatIsGround);

        #region Ground Drag
        if ((state == MovementState.walking || state == MovementState.sprinting) && grounded) rb.drag = groundDrag;
        else rb.drag = 0;
        #endregion

    }

    private void MyInput()
    {
        isSprinting = input.i_sprint;
        moveDir.x = input.movementInput.x;
        moveDir.z = input.movementInput.y;

        if (input.i_jump && readyToJump && grounded)
        {

            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldwn);
        }

    }

    private void StateHandler()
    {

        if (isSprinting)
        {
            state = MovementState.sprinting;
            currentSpeed = sprintSpeed;
        }

        else if (grounded)
        {
            state = MovementState.walking;
            currentSpeed = walkSpeed;
        }

        else
        {
            state = MovementState.air;
            currentSpeed = walkSpeed;
        }
    }

    private void Movement()
    {

        moveDir = orientation.forward * input.movementInput.y + orientation.right * input.movementInput.x;

        if (OnSlope() && !extitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDir) * currentSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        //Realistic Air Speed
        if(currentSpeed>0) airMultiplayer = 2 / currentSpeed;
        //

        if (grounded)
            rb.AddForce(moveDir.normalized * currentSpeed * 10f, ForceMode.Force);

        else if (!grounded)
            rb.AddForce(moveDir.normalized * currentSpeed * 10f * airMultiplayer, ForceMode.Force);

        rb.useGravity = !OnSlope();

    }

    private void SpeedControl()
    {
        if (currentSpeed < 0) currentSpeed = 0;

        if (OnSlope() && !extitingSlope)
        {
            if (rb.velocity.magnitude > currentSpeed)
                rb.velocity = rb.velocity.normalized * currentSpeed;
        }

        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > currentSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * currentSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }

        }
    }

    private void Jump()
    {
        extitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        extitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
    #endregion


    #region Health
    public void Heal(float _amount){

        currentHealth+=_amount;

        if(currentHealth>maxHealth) currentHealth = maxHealth;
    }

    public override void TakeDamage(float _damage)
    {
        if(_damage>=maxHealth) currentHealth=1f;
        else currentHealth-=_damage;

        if(currentHealth<=0) Die();
    }

    public override void Die()
    {
        
        SceneManager.LoadScene(0);

    }
    #endregion

    private void Debug(){

        if(Input.GetKeyDown(KeyCode.Alpha1)) SceneManager.LoadScene(0);
        else if(Input.GetKeyDown(KeyCode.Alpha2))SceneManager.LoadScene(1);


        if(Input.GetKeyDown(KeyCode.T)) TakeDamage(Random.Range(10,35));

    }

    //--------------------------------------------------------------------------------------------------------------------------------

}
