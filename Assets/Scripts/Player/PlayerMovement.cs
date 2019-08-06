using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public bool debug = false;
    [Tooltip("Whether the player is capable of moving or not. True = can move.")]
    public bool canMove = true;

    [Tooltip("How quickly the player reaches its max speed.")]
    [SerializeField]
    private float acceleration = 0.1f;
    [Tooltip("How quickly the player returns to zero if not moving.")]
    [SerializeField]
    private float deceleration = 0.1f;
    [Tooltip("If speed is less than or equal to this value then snap to zero speed.")]
    [SerializeField]
    private float snapToZeroThreshold = 0.05f;
    [Tooltip("The maximum speed the player can achieve.")]
    [SerializeField]
    private float maxSpeed = 1f;
    [Tooltip("The minimum input threshold. If the input received from the stick is smaller than this value, it’s ignored.")]
    [SerializeField]
    [Range (0f, 1f)]
    private float deadZone = 0.15f;

    private float xInput = 0f;
    private float zInput = 0f;
    private Vector3 dir; // The direction of the movement
    private Rigidbody rb;

    // Burst attributes
    private bool burst = false; // Only apply burst forces if true
    private bool onlyBurst = false; // Don't apply movement forces when true
    private float maxBurstForce = 0f; // The peak force of the burst
    private float burstAcceleration = 0f; // The rate at which the player will reach its burst force
    private float burstDuration = 0f; // Time in seconds for the burst
    private float burstTimer = 0f; // Timer used to time how long a burst has occurred
    private Vector3 burstDir; // The direction of the burst force

    private void Start(){
        dir = new Vector3(0f, 0f, 0f);
        rb = GetComponent<Rigidbody>();
    }

    private void Update(){
        GetInput();

        // Snap velocity to zero if speed is within threshold
        if(rb.velocity.magnitude <= snapToZeroThreshold)
            rb.velocity = Vector3.zero;

        if(debug){
            Debug.Log("Input direction: " + dir);
        }
    }

    private void FixedUpdate() {
        if(!onlyBurst) MovePlayer();
        if(burst) BurstPlayer();
    }

    /******************************************************************
     *  MovePlayer()
     *   Accelerate the movement force in the direction of movement.
     *   If the player isn't moving then set the force to zero. Clamp
     *   the force to be less than the player's max speed. Then move
     *   the player's position to the new location determined by the 
     *   force. Movement is interpolated to make it smoother.
     *****************************************************************/
    private void MovePlayer(){
        if(dir.magnitude != 0f){ // Accelerate velocity in movement direction
            rb.velocity += acceleration * dir;

            // Clamp velocity to max speed
            rb.velocity  = Vector3.ClampMagnitude(rb.velocity , maxSpeed * dir.magnitude);
        }
        else if(dir.magnitude == 0f && rb.velocity.magnitude > deceleration){ // Not moving, so decelerate
            rb.velocity -= deceleration * rb.velocity.normalized;
        }
        else{ // Finished decelerating, set velocity to zero
            rb.velocity = Vector3.zero;
        }
    }

    /******************************************************************
     *  BurstPlayer()
     *   Perform movement of the player similarly to MovePlayer().
     *   But do so in a burst of force that lasts for a specified
     *   duration. A burst is initially set up by a call to the
     *   function ApplyBUrstForce(), which sets the attributes for a
     *   burst to occur. Bursts can be used for rolling, knockback, or
     *   anything that requires a sudden burst of force.
     *****************************************************************/
    private void BurstPlayer(){
        // Bursting time is up or burst attributes are invalid
        if(maxBurstForce == 0f || burstAcceleration == 0f || burstDuration == 0f || burstDir == Vector3.zero){
            burst = false;
            onlyBurst = false;
            return;
        }

        // Accelerate
        rb.velocity += burstAcceleration * burstDir.normalized;

        // Clamp
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxBurstForce);

        // Increment the timer, set bools accordingly
        burstTimer += Time.deltaTime;
        burst = burstTimer < burstDuration;
        onlyBurst = (burstTimer < burstDuration) && onlyBurst;
    }

    /******************************************************************
     *  GetInput()
     *   Find the x and z inputs for the player's movement. Then set
     *   the direction vector. The input is not gotten if the player
     *   cannot move.
     *****************************************************************/
    private void GetInput(){
        // Return if the player can't move
        if(!canMove) {
            dir = Vector3.zero;
            return;
        }
        // Get inputs
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
        dir = dir.WithX(xInput).WithZ(zInput);
        // Ensure that movement direction never exceeds 1
        if(dir.magnitude > 1f)
            dir = dir.normalized;


        // Ensure the input goes past the dead zone
        Vector2 stickInput = new Vector2(xInput, zInput);
        if(stickInput.magnitude < deadZone)
            dir = Vector3.zero;
    }

    public void SetMaxSpeed(float new_max){
        maxSpeed = new_max;
    }

    /******************************************************************
     *  FreezePlayerMovement()
     *   Prevent the player from moving by setting canMove to false
     *   and setting movement direction to zero.
     *****************************************************************/
    public void FreezePlayerMovement(){
        canMove = false;
        dir = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    public Vector3 GetMoveDirection(){
        return dir.normalized;
    }

    public bool IsMoving(){
        return rb.velocity.magnitude >= snapToZeroThreshold;
    }

    public float GetSpeed(){
        return rb.velocity.magnitude;
    }

    public float GetMaxSpeed(){
        return maxSpeed;
    }

    public void ApplyBurstForce(float max, float accel,float duration, bool exclusive){
        onlyBurst = exclusive;
        maxBurstForce = max;
        burstAcceleration = accel;
        burstDuration = duration;

        burstTimer = 0f;
        burst = true;
        burstDir = dir;
    }

    public void ApplyBurstForce(float max, float accel,float duration, bool exclusive, Vector3 direction){
        onlyBurst = exclusive;
        maxBurstForce = max;
        burstAcceleration = accel;
        burstDuration = duration;
        burstDir = direction;

        burstTimer = 0f;
        burst = true;
    }
}
