using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/MeleeAbility")]
public class MeleeAbility : PlayerAbility
{
    [Tooltip("The distance from the origin toward the aim direction that the attack hitbox will be.")]
    [SerializeField]
    protected float range;
    [Tooltip("The melee attack's hitbox. Must be a sphere, box or capsule collider.")]
    [SerializeField]
    protected Collider hitBox;
    [Tooltip("The amount of hit point damage this attack will do.")]
    [SerializeField]
    protected int power = 1;

    protected Vector3 attackOrigin;
    protected Transform playerOrigin;
    protected PlayerAim aimDir;
    protected Health hp;

    public override void Initialize(){
        hp = player.GetComponent<Health>();
        aimDir = player.GetComponent<PlayerAim>();
        playerOrigin = player.transform;
    }

    public override void ActivateAbility(){
        Collider[] hits = new Collider[1];

        // Find the origin of the hitBox
        attackOrigin = playerOrigin.position + (aimDir.GetAimDirection() * range);

        // Perform the check of the given collider type
        if(hitBox.GetType() == typeof(SphereCollider)){
            hits = Physics.OverlapSphere(attackOrigin, ((SphereCollider)hitBox).radius);
        }else if(hitBox.GetType() == typeof(BoxCollider)){
            // TODO :: hits = Physics.OverlapBox();
        }else if(hitBox.GetType() == typeof(CapsuleCollider)){
            // TODO :: hits = Physics.OverlapCapsule();
        }else{
            return;
        }

        foreach (Collider rch in hits)
        {
            // Skip hits that don't have health or are the player
            if(!rch.gameObject.GetComponent<Health>() || rch.transform == playerOrigin)
                continue;
            Health enemyHP = rch.gameObject.GetComponent<Health>();
            
            enemyHP.TakeDamageInvincibility(power);
        }
    }
}
