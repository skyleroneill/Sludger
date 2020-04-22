using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnConditions
{
    public bool onImpact;
    public bool onHurt;
}

[System.Serializable]
public struct SpawnedObjects
{
    public GameObject spawnObject;
    [Tooltip("The conditions on which a projectile will spawn. Checking more than one can result in multiple drops.")]
    public SpawnConditions spawnConditions;
}

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Tooltip("The speed this bullet will move at.")]
    [SerializeField]
    protected float speed = 10f;
    [Tooltip("The amount of hit point damage this attack will do.")]
    [SerializeField]
    protected int power = 1;
    [Tooltip("The amount of knockback this projectile will appy when it hits an object that can be knocked back. Zero or fewer results in no knockback.")]
    [SerializeField]
    protected float knockbackForce = 0f;
    [Tooltip("The amount of time in seconds that objects hit by this projectile will be stunned. Values of zero or less result in no hitstun.")]
    [SerializeField]
    protected float hitstunDuration = 0f;
    [Tooltip("The layers that this projectile will ignore.")]
    [SerializeField]
    protected List<int> ignoreLayers;
    [SerializeField]
    protected float lifeTime = 5f;
    [Tooltip("Should this projectile move forward automatically. Or should it wait for a direction to be given.")]
    [SerializeField]
    protected bool moveForward = false;
    [Tooltip("Should this projectile be destroyed when it hits an object.")]
    [SerializeField]
    protected bool destroyOnHit = true;
    [Tooltip("The game objects this projectile will spawn.")]
    [SerializeField]
    private SpawnedObjects[] spawnObjects;


    protected Vector3 dir;
    protected Rigidbody rb;
    protected GameObject shooter;

    private void Start(){
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
        if(moveForward) dir = transform.forward;
    }

    private void Update(){
        if(dir != null){
            rb.velocity = dir * speed;
        }
    }

    private void OnCollisionEnter(Collision col){
        HitBehaviour(col.gameObject);
    }

    private void OnTriggerEnter(Collider col){
        HitBehaviour(col.gameObject);
    }

    private void HitBehaviour(GameObject hit){
        // Don't collide with the object that shot it
        if(shooter == hit) return;

        // Don't collide with objects on an ignoreLayer
        foreach(int layer in ignoreLayers){
            if(hit.layer == layer) return;
        }

        // Drop Impact Spawnables
        if (spawnObjects.Length > 0)
        {
            foreach(SpawnedObjects obj in spawnObjects)
            {
                if (obj.spawnConditions.onImpact) GameObject.Instantiate(obj.spawnObject, transform.position, transform.rotation, null);
            }
        }

        int damageDealt = 0;

        // Damage the collided object if it has health
        if(hit.GetComponent<Health>()){
            // If the damage dealt is greater than 0, destroy this projectile
            damageDealt = hit.GetComponent<Health>().TakeDamageInvincibility(power);
        }

        // If no damage was dealt then the don't do anything
        if(damageDealt == 0)
        {
            // Destroy the projectile on hit
            if (destroyOnHit) Destroy(gameObject);
            return;
        }

        // Drop Hurt Spawnables
        if (spawnObjects.Length > 0)
        {
            foreach (SpawnedObjects obj in spawnObjects)
            {
                if (obj.spawnConditions.onHurt) GameObject.Instantiate(obj.spawnObject, transform.position, transform.rotation, null);
            }
        }

        // Only do the following if the attack dealt damage
        if (damageDealt > 0)
        {
            // Apply knockback force to the target that was hit if it has a HitstunAndKnockback component
            if (hit.GetComponent<HitstunAndKnockback>() && knockbackForce > 0f)
            {
                if (rb.velocity != Vector3.zero) // Apply knockback in direction of moving projectiles
                    hit.GetComponent<HitstunAndKnockback>().Knockback(rb.velocity, knockbackForce);
                else // Apply knockback in direction of hit from the projectile
                    hit.GetComponent<HitstunAndKnockback>().Knockback(hit.transform.position - transform.position, knockbackForce);
            }

            // Apply hitstun to the target that was hit if it has a HitstunAndKnockback component
            if (hit.GetComponent<HitstunAndKnockback>())
            {
                hit.GetComponent<HitstunAndKnockback>().Hitstun(hitstunDuration);

                // NPC knockback/stun
                if(rb.velocity != Vector3.zero) // Apply knockback in direction of moving projectiles
                    hit.GetComponent<HitstunAndKnockback>().NPCKnockbackAndHitstun(rb.velocity, knockbackForce, hitstunDuration);
                else // Apply knockback in direction of hit from the projectile
                    hit.GetComponent<HitstunAndKnockback>().NPCKnockbackAndHitstun(hit.transform.position - transform.position, knockbackForce, hitstunDuration);
            }
        }

        // Destroy the projectile on hit
        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 d){
        dir = d;
    }

    public void SetSpeed(float s){
        speed = s;
    }

    public void SetPower(int p){
        power = p;
    }

    public void SetShooter(GameObject s){
        shooter = s;
    }

    public void AddIgnoreLayer(int layer){
        ignoreLayers.Add(layer);
    }
}
