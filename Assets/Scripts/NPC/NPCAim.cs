using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectTargeting))]
[RequireComponent(typeof(NPCMove))]
public class NPCAim : MonoBehaviour
{
    public enum IdleType {MovementDirection, RandomDirection, Clockwise, CounterClockwise, LastDirection};

    public bool debug = false;

    [Tooltip("Should this object aim at its target or perform its idle behavior instead.")]
    [SerializeField]
    private bool trackTarget = true;
    [SerializeField]
    [Range (0f, 1f)]
    private float interpolant = 0.95f;
    [SerializeField]
    private IdleType idleType;
    [Tooltip("The amount of time between changing to a new random direction when idle type is random.")]
    [SerializeField]
    private float randomDirectionTime = 3f;

    // Variables used by the idle aim types
    private bool newRandomDir = true;
    private Vector3 randomDir = Vector3.zero;

    // True when rotating to a set direction
    // Disables both idle and tracking behavior
    private bool overrideAim = false;

    private Vector3 aimDir;
    //private Vector3 lastDir;
    private ObjectTargeting ot;
    private NPCMove move;

    private void Start(){
        aimDir = transform.forward;
        ot = GetComponent<ObjectTargeting>();
        move = GetComponent<NPCMove>();
    }

    private void FixedUpdate(){
        if(debug){
            // Draw a line pointing in the aiming direction
            Debug.DrawRay(transform.position.WithAddedY(0.01f), aimDir.normalized, Color.green, 0f, false);
        }

        if(overrideAim){
            return;
        }else if(trackTarget)
            TargetAim();
        else
            IdleAim();
    }

    private void TargetAim(){
        Vector3 newDir = (ot.GetCurrentTarget().transform.position - transform.position).normalized;
        aimDir = Vector3.Slerp(aimDir, newDir, interpolant).WithY(0f);
    }

    private void IdleAim(){
        if(idleType == IdleType.MovementDirection){
            // Do not face movement direction if not moving
            if(move.GetDirection() == Vector3.zero) return;

            // Face direction of movement
            aimDir = Vector3.Slerp(aimDir, move.GetDirection(), interpolant);
        } else if(idleType == IdleType.RandomDirection){
            // If it is time to generate a new random direction
            if(newRandomDir){
                randomDir = Vector3.zero.WithX(Random.Range(-1f, 1f)).WithZ(Random.Range(-1f, 1f));
                StartCoroutine(Wait());
            }

            // If the generated random direction is zero, then remain facing the previous direction
            if(randomDir == Vector3.zero) randomDir = aimDir;

            // Normalize the random direction
            Vector3.Normalize(randomDir);

            // Face the random direction
            aimDir = Vector3.Slerp(aimDir, randomDir, interpolant);

        } else if(idleType == IdleType.Clockwise){
            Quaternion q = Quaternion.Euler(0f, 1f, 0f);
            aimDir = q * aimDir;
        } else if(idleType == IdleType.CounterClockwise){
            Quaternion q = Quaternion.Euler(0f, -1f, 0f);
            aimDir = q * aimDir;
        } else if(idleType == IdleType.LastDirection){
            // Don't update aim direction
            return;
        }
    }

    public void AimAtDirection(Vector3 dir){
        aimDir = dir;
        overrideAim = true;
    }

    public void AimAtTarget(bool aimAtTarget){
        overrideAim = false;
        trackTarget = aimAtTarget;
    }

    public Vector3 GetAimDirection(){
        return aimDir.normalized;
    }

    public void SetIdleType(IdleType type){
        idleType = type;
    }

    IEnumerator Wait(){
        newRandomDir = false;
        yield return new WaitForSeconds(randomDirectionTime);
        newRandomDir = true;
    }
}
