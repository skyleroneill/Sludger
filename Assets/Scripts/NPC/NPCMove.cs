using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ObjectTargeting))]
[RequireComponent(typeof(NPCAim))]
public class NPCMove : MonoBehaviour
{
    public enum Direction {forward, backward, left, right, forwardLeft, forwardRight, backwardLeft, backwardRight, stop};

    public bool debug = false;

    [SerializeField]
    private Transform destination;
    [SerializeField]
    private bool traversePath = true;
    [SerializeField]
    private bool stopMovement = false;
    [SerializeField]
    private float auxilaryMovementSpeed = 2f;

    private Direction auxDirection = Direction.stop;
    private Vector3 auxMovement = Vector3.zero;
    private NavMeshAgent nma;
    private ObjectTargeting ot;
    private NPCAim npcAim;

    private void Start(){
        nma = GetComponent<NavMeshAgent>();
        ot = GetComponent<ObjectTargeting>();
        npcAim = GetComponent<NPCAim>();

        // Do not modify object's rotation or position
        // Position will be manually set using FixedUpdate()
        nma.updateRotation = false;
        nma.updatePosition = false;

        // If destination isn't preset, then use default target
        if(!destination)
            destination = ot.GetTarget(0).transform;
        
        SetDestination();
    }

    private void Update(){
        if(debug){
            // Toggle path traversal
            if(Input.GetKeyDown(KeyCode.L)) SetTraversePath(!traversePath);
            // Toggle stopping movement
            if(Input.GetKeyDown(KeyCode.M)) SetIsStopped(!nma.isStopped);

            Debug.Log("Travere Path: " + traversePath);
            Debug.Log("Is Stopped: " + nma.isStopped);
            Debug.Log("Path Length: " + CalculatePathLength());
        }

        nma.isStopped = stopMovement;

        SetDestination();

        if(traversePath && !stopMovement){
            SetDestination();
        } else if(!stopMovement){
            // Determine the direction to face
            DetermineDirection(auxDirection);
            nma.velocity = auxMovement * auxilaryMovementSpeed;
        } else if(stopMovement){
            // Clear path and set velocity to zero
            //ClearPath();
            nma.velocity = Vector3.zero;
        }
    }

    private void FixedUpdate(){
        if(!stopMovement)
            transform.position += nma.velocity * Time.deltaTime;

        nma.nextPosition = transform.position;
    }

    private void SetDestination(){
        // Ensure that destination is valid
        if(!destination) return;

        // Set the new destination
        nma.SetDestination(destination.position);
    }

    private void DetermineDirection(Direction dir){
        // Set movement according to direction
        if(dir == Direction.forward) auxMovement = npcAim.GetAimDirection();
        else if(dir == Direction.backward) auxMovement = -npcAim.GetAimDirection();
        else if(dir == Direction.left) auxMovement = -RelativeRight();
        else if(dir == Direction.right) auxMovement = RelativeRight();
        else if(dir == Direction.forwardLeft) auxMovement = npcAim.GetAimDirection() - RelativeRight();
        else if(dir == Direction.forwardRight) auxMovement = npcAim.GetAimDirection() + RelativeRight();
        else if(dir == Direction.backwardLeft) auxMovement = -npcAim.GetAimDirection() - RelativeRight();
        else if(dir == Direction.backwardRight) auxMovement = -npcAim.GetAimDirection() + RelativeRight();
        else if(dir == Direction.stop) auxMovement = Vector3.zero;

        // Normalize the direction
        Vector3.Normalize(auxMovement);
    }

    private Vector3 RelativeRight(){
        // Find right relative to the current aim direction
        Vector3 right = Vector3.zero;
        right = right.WithX(npcAim.GetAimDirection().z).WithZ(-npcAim.GetAimDirection().x);
        return right;
    }

    public bool SetIsStopped(bool stop){
        // Toggle whether or not the object will move to its destination
        stopMovement = stop;
        return stopMovement;
    }

    public bool IsStopped(){
        return stopMovement;
    }

    public void ClearPath(){
        if(nma.hasPath)
            nma.ResetPath();
    }

    public Vector3 GetDirection(){
        return nma.velocity.normalized;
    }

    public void SetMovement(Direction dir){
        auxDirection = dir;
        ClearPath();
        DetermineDirection(dir);
    }

    public bool SetTraversePath(bool tp){
        traversePath = tp;
        return traversePath;
    }

    public void SetAuxMovementSpeed(float speed){
        auxilaryMovementSpeed = speed;
    }

    public void SetPathMovementSpeed(float speed){
        nma.speed = speed;
    }

    public void ChangeDestination(int target = -1){
        // Change destination to current target if no other was specified
        if(target == -1) 
            destination = ot.GetTarget(ot.GetCurrentTargetIndex()).transform;
        else
            destination = ot.GetTarget(target).transform;
    }

    public float CalculatePathLength(){
        // If there is currently no path, then return
        if(!nma.hasPath) return 0f;

        // If there is a direct path to the target simply return the distance to it
        if(nma.path.corners.Length == 2){
            return Vector3.Distance(nma.path.corners[0], nma.path.corners[1]);
        }
    
        float dist = 0f;
        for(int i = 0; i <= nma.path.corners.Length - 2; i++)
        {
            // Add the distance between each corner and the one that follows
            dist += Vector3.Distance(nma.path.corners[i], nma.path.corners[i + 1]);
        }

        return dist;
    }
}
