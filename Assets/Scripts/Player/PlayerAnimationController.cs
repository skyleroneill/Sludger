using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAim))]
public class PlayerAnimationController : MonoBehaviour
{
    [Tooltip("Controls the playback speed of the run animation.")]
    [SerializeField]
    private float runAnimationSpeedMultiplier = 1.5f;

    PlayerMovement pMove;
    PlayerAim pAim;
    Quaternion lookDir;
    Quaternion moveDir;
    Animator aniCon;

    private void Start(){
        pMove = GetComponent<PlayerMovement>();
        pAim = GetComponent<PlayerAim>();
        lookDir = Quaternion.LookRotation(pAim.GetAimDirection());
        moveDir = Quaternion.LookRotation(pMove.GetMoveDirection());
        aniCon = GetComponentInChildren<Animator>();
    }

    private void Update(){
        SetMoving();
        SetAim();
    }

    private void SetAim(){
        lookDir = Quaternion.LookRotation(pAim.GetAimDirection());

        if (lookDir.eulerAngles.y >= 337.5 || lookDir.eulerAngles.y < 22.5){
            // Up
            aniCon.SetBool("up", true);
            aniCon.SetBool("down", false);
            aniCon.SetBool("left", false);
            aniCon.SetBool("right", false);
        }
		else if (lookDir.eulerAngles.y >= 22.5 && lookDir.eulerAngles.y < 67.5){
			// Up Right
            aniCon.SetBool("up", true);
            aniCon.SetBool("down", false);
            aniCon.SetBool("left", false);
            aniCon.SetBool("right", true);
        }
		else if (lookDir.eulerAngles.y >= 67.5 && lookDir.eulerAngles.y < 112.5){
			// Right
            aniCon.SetBool("up", false);
            aniCon.SetBool("down", false);
            aniCon.SetBool("left", false);
            aniCon.SetBool("right", true);
        }
		else if (lookDir.eulerAngles.y >= 112.5 && lookDir.eulerAngles.y < 157.5){
			// Down Right
            aniCon.SetBool("up", false);
            aniCon.SetBool("down", true);
            aniCon.SetBool("left", false);
            aniCon.SetBool("right", true);
        }
		else if (lookDir.eulerAngles.y >= 157.5 && lookDir.eulerAngles.y < 202.5){
			// Down
            aniCon.SetBool("up", false);
            aniCon.SetBool("down", true);
            aniCon.SetBool("left", false);
            aniCon.SetBool("right", false);
        }
		else if (lookDir.eulerAngles.y >= 202.5 && lookDir.eulerAngles.y < 247.5){
			// Down Left
            aniCon.SetBool("up", false);
            aniCon.SetBool("down", true);
            aniCon.SetBool("left", true);
            aniCon.SetBool("right", false);
        }
		else if (lookDir.eulerAngles.y >= 247.5 && lookDir.eulerAngles.y < 292.5){
			// Left
            aniCon.SetBool("up", false);
            aniCon.SetBool("down", false);
            aniCon.SetBool("left", true);
            aniCon.SetBool("right", false);
        }
		else if (lookDir.eulerAngles.y >= 292.5 && lookDir.eulerAngles.y < 337.5){
			// Up Left
            aniCon.SetBool("up", true);
            aniCon.SetBool("down", false);
            aniCon.SetBool("left", true);
            aniCon.SetBool("right", false);
        }
    }

    private void SetMoving(){
        aniCon.SetBool("moving",pMove.IsMoving());

        if(pMove.GetMoveDirection() != Vector3.zero)
            moveDir = Quaternion.LookRotation(pMove.GetMoveDirection());

        float speedRatio = pMove.GetSpeed() / pMove.GetMaxSpeed() * runAnimationSpeedMultiplier;

        // Play the movement animation backwards if the player is looking
        // in the opposite direction of their movement.
        if(Mathf.Abs(lookDir.eulerAngles.y - moveDir.eulerAngles.y) >= 90f)
            speedRatio *= -1f;

        // If speed would be 0, then make it one
        // This prevents animations from getting stuck
        if(speedRatio == 0f)
            aniCon.SetFloat("speed", 1f);
        else
            aniCon.SetFloat("speed", speedRatio);
    }

    public void SetBoolParameter(string parameter, bool val){
        aniCon.SetBool(parameter, val);
    }

    public void SetBoolEnabledForTime(string parameter, float time){
        aniCon.SetBool(parameter, true);
        StartCoroutine(WaitToDisable(parameter, time));
    }

    public void EndRoll(){
        aniCon.SetBool("rolling", false);
    }

    IEnumerator WaitToDisable(string parameter, float time){
        yield return new WaitForSeconds(time);
        aniCon.SetBool(parameter, false);
    }
}
