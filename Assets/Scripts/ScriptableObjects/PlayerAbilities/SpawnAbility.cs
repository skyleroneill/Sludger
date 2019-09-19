using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SpawnAbility")]
public class SpawnAbility : PlayerAbility
{
    [Tooltip("The game object that this ability will spawn.")]
    [SerializeField]
    protected GameObject spawn;
    [Tooltip("The distance from the player that the item will spawn.")]
    [SerializeField]
    protected float spawnDistance;
    [Tooltip("Should the spawned object be rotated to face the direction the player is facing?")]
    [SerializeField]
    protected bool rotateToPlayerAimDirection;
    //[Tooltip("Should the object be spawned even if its collider overlaps the location where it will be spawned? True if you wish the item to be spawned regardless of overlaps. False if overlaps prevent the item from spawning.")]
    //[SerializeField]
    //protected bool spawnOnOverlap;

    protected PlayerAim aimDir;
    protected Health hp;

    public override void Initialize(){
        hp = player.GetComponent<Health>();
        aimDir = player.GetComponent<PlayerAim>();
    }

    public override void ActivateAbility(){
        hp.TakeDamage(cost);

        // Determine where the item will spawn
        Vector3 spawnPoint = player.transform.position + (aimDir.GetAimDirection() * spawnDistance);

        // Determine the direction the object will face
        Quaternion direction = Quaternion.identity;
        if (rotateToPlayerAimDirection)
        {
            direction = Quaternion.LookRotation(aimDir.GetAimDirection());
        }

        // Spawn the object
        Instantiate(spawn, spawnPoint, direction);
    }
}
