using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAim : MonoBehaviour
{
    public bool debug = false;

    [Tooltip("The minimum input threshold. If the input received from the stick is smaller than this value, it’s ignored.")]
    [SerializeField]
    [Range (0f, 1f)]
    private float deadZone = 0.15f;

    private float xInput = 0f;
    private float zInput = 0f;
    private Vector3 dir;
    private Vector3 lastDir; // Last direction the player faced, used as default direction
    private PlayerMovement pm;

    private void Start(){
        // Initial direction is downward
        dir = new Vector3(0f, 0f, -1f);
        lastDir = dir;
        pm = gameObject.GetComponent<PlayerMovement>();
    }

    private void Update() {
        // Get the x and z input
        xInput = Input.GetAxis("HorizontalAim");
        zInput = Input.GetAxis("VerticalAim");

        // The stick input as a vector
        Vector2 stickInput = new Vector2(xInput, zInput);

        if(stickInput.magnitude < deadZone && pm.GetSpeed() > 0f) // Aim in movement direction if not aiming
            dir = pm.GetMoveDirection();
        else if(stickInput.magnitude < deadZone) // Ensure the input goes past the dead zone
            dir = lastDir;
        else // Aim in inputted direction
            dir = dir.WithX(xInput).WithZ(zInput).normalized;

        // Ensure that dir can never be zero
        if(dir == Vector3.zero)
            dir = lastDir;

        // Set lastDir to the direction if the direction is valid
        if(dir != Vector3.zero)
            lastDir = dir;
        
        if(debug){
            // Draw a line pointing in the aiming direction
            Debug.DrawRay(transform.position.WithAddedY(0.01f), dir, Color.green, 0f, false);
        }
    }

    /******************************************************************
     *  GetAimDirection()
     *   Return the normalized direction in which the player is facing
     *****************************************************************/
    public Vector3 GetAimDirection(){
        return lastDir.normalized;
    }
}
