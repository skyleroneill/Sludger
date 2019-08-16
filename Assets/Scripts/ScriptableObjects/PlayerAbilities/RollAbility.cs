using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/RollAbility")]
public class RollAbility : PlayerAbility
{
    [Tooltip("The maximum force applied during the roll.")]
    [SerializeField]
    protected float rollForce;
    [Tooltip("How quickly the force of the roll builds up.")]
    [SerializeField]
    protected float rollAcceleration;
    [Tooltip("How long, in seconds, the roll will last.")]
    [SerializeField]
    protected float rollDuration;

    protected PlayerMovement pm;

    public override void Initialize(){
        pm = player.GetComponent<PlayerMovement>();
        pac = player.GetComponent<PlayerAnimationController>();
    }

    public override void ActivateAbility(){
        // Apply roll force only if the player is already moving
        if(pm.IsMoving()){
            pm.ApplyBurstForce(rollForce, rollAcceleration, rollDuration, true);

            // Set the animation to play if the player has an animation controller script
            if(pac)
                pac.SetBoolEnabledForTime(animationParameter, rollDuration);
        }
    }

    public override void StartAnimationEvent(){
        if(player.GetComponent<Health>())
            player.GetComponent<Health>().SetIFrames(true);
    }

    public override void EndAnimationEvent(){
        if(player.GetComponent<Health>())
            player.GetComponent<Health>().SetIFrames(false);
    }
}
