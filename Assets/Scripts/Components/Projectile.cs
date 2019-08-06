using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Tooltip("The speed this bullet will move at.")]
    [SerializeField]
    protected float speed = 10f;
    [Tooltip("The amount of hit point damage this attack will do.")]
    [SerializeField]
    protected int power = 1;
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

    private void OnCollisionStay(Collision col){
        HitBehaviour(col.gameObject);
    }

    private void OnTriggerEnter(Collider col){
        HitBehaviour(col.gameObject);
    }

    private void OnTriggerStay(Collider col){
        HitBehaviour(col.gameObject);
    }

    private void HitBehaviour(GameObject hit){
        // Don't collide with the object that shot it
        if(shooter == hit) return;

        // Don't collide with objects on an ignoreLayer
        foreach(int layer in ignoreLayers){
            if(hit.layer == layer) return;
        }

        // Damage the collided object if it has health
        if(hit.GetComponent<Health>()){
            // If the damage dealt is greater than 0, destroy this projectile
            if(hit.GetComponent<Health>().TakeDamageInvincibility(power) > 0 && destroyOnHit)
                Destroy(gameObject); 
        }else if(destroyOnHit)
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
