using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbility : ScriptableObject
{
    public bool debug = false;
    [Tooltip("How much of a resource, such as health, is expended by using this ability.")]
    [SerializeField]
    protected int cost;
    [Tooltip("The time between uses of this ability.")]
    [SerializeField]
    protected float cooldown;
    [Tooltip("The name of the animation parameter for this ability.")]
    [SerializeField]
    protected string animationParameter;

    protected GameObject player;
    protected PlayerAnimationController pac;

    public void SetPlayer(GameObject p){
        player = p;
    }

    public abstract void Initialize();
    public abstract void ActivateAbility();

    public float GetCooldown(){
        return cooldown;
    }

    public virtual void StartAnimationEvent(){
        return;
    }

    public virtual void EndAnimationEvent(){
        return;
    }
}
