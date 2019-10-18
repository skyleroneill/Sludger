using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitstunAndKnockback : MonoBehaviour
{
    public bool debug = false;
    [Tooltip("The material of this material when it is not in hitstun.")]
    [SerializeField]
    private Material defaultMaterial;
    [Tooltip("The material of this material when it is in hitstun.")]
    [SerializeField]
    private Material hitMaterial;

    private bool inHitstun = false;
    private SpriteRenderer sprite;

    private void Start(){
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    public void Knockback(Vector3 dir, float force){
        // Don't apply knockback when the gameobject is on invincibility time
        if (gameObject.GetComponent<Health>() && gameObject.GetComponent<Health>().IsInITime())
            return;

        // Player and props use Rigidbodies
        if(gameObject.GetComponent<Rigidbody>()){
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.AddForce(dir.normalized * force, ForceMode.Impulse);
            if (debug) Debug.Log("Applied knockback to " + gameObject.name);
        }
    }

    public void Hitstun(float time){
        // Don't apply hitstun when:
        //  time is invalid
        //  We're already in hitstun
        //  We have health and are on invincibility time
        if(time <= 0f || inHitstun || (gameObject.GetComponent<Health>() && gameObject.GetComponent<Health>().IsInITime()))
            return;
        
        inHitstun = true;

        // Stop the player from moving
        if(gameObject.GetComponent<PlayerMovement>())
            gameObject.GetComponent<PlayerMovement>().SetCanMove(false);

        // Stop the player from using abilities
        if(gameObject.GetComponent<AbilitySlots>())
            gameObject.GetComponent<AbilitySlots>().SetCanUseAbilities(false);

        // Apply the hit material to the sprite
        if(sprite && hitMaterial)
            sprite.material = hitMaterial;

        StartCoroutine(StunTimer(time));
    }

    public bool IsInHitstun(){
        return inHitstun;
    }

    IEnumerator StunTimer(float time){
        yield return new WaitForSeconds(time);
        inHitstun = false;

        if(gameObject.GetComponent<PlayerMovement>())
            gameObject.GetComponent<PlayerMovement>().SetCanMove(true);

        if(gameObject.GetComponent<AbilitySlots>())
            gameObject.GetComponent<AbilitySlots>().SetCanUseAbilities(true);

        if(sprite && defaultMaterial)
            sprite.material = defaultMaterial;
    }
}
