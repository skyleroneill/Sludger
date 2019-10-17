using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitstunAndKnockback : MonoBehaviour
{
    [SerializeField]
    private float hitstunDuration = 0f;
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private Material hitMaterial;

    private bool inHitstun = false;
    private SpriteRenderer sprite;

    private void Start(){
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    public void Knockback(Vector3 dir, float force){
        // Player and props use Rigidbodies
        if(gameObject.GetComponent<Rigidbody>()){
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.AddForce(dir.normalized * force, ForceMode.Impulse);
        }
    }

    public void Hitstun(){
        if(hitstunDuration <= 0f)
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

        StartCoroutine(StunTimer(hitstunDuration));
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
