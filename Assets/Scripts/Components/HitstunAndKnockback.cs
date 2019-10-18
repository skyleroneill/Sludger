using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

        // NPCs don't use this Knockback Function
        if (gameObject.GetComponent<NavMeshAgent>())
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

        // NPCs don't use this hitstun Function
        if (gameObject.GetComponent<NavMeshAgent>())
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

    public void NPCKnockbackAndHitstun(Vector3 dir, float force, float time = 0.1f)
    {
        // Don't apply knockback/hitstun when already stunned or in i time the gameobject is on invincibility time
        if (inHitstun || (gameObject.GetComponent<Health>() && gameObject.GetComponent<Health>().IsInITime()))
            return;

        // Players and props don't use this Knockback/stun function
        if (!gameObject.GetComponent<NavMeshAgent>())
            return;
        // 0.1 is minimum time for hitstun
        if (time < 0.1f) time = 0.1f;

        inHitstun = true;

        // Apply the hit material to the sprite
        if (sprite && hitMaterial)
            sprite.material = hitMaterial;

        // Pause brain
        if (gameObject.GetComponent<NPCBrain>())
        {
            gameObject.GetComponent<NPCBrain>().TogglePauseBrain(true);
            gameObject.GetComponent<NPCBrain>().RestartCurrentBehavior();
        }

        // Stop movement
        if (gameObject.GetComponent<NPCMove>())
            gameObject.GetComponent<NPCMove>().SetIsStopped(true);

        // Aim in last dir
        if (gameObject.GetComponent<NPCAim>())
            gameObject.GetComponent<NPCAim>().SetIdleType(NPCAim.IdleType.LastDirection);

        // Stop shooting
        if (gameObject.GetComponent<NPCShoot>())
            gameObject.GetComponent<NPCShoot>().StopBurst();

        // Stop being kinematic, allow forces to be added
        // Then add the force
        if (gameObject.GetComponent<Rigidbody>())
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().AddForce(dir.normalized * force, ForceMode.Impulse);
        }

        // Start up the stun timer
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

        // Restart NPC's brain
        if (gameObject.GetComponent<NPCBrain>())
        {
            gameObject.GetComponent<NPCBrain>().TogglePauseBrain(false);
            gameObject.GetComponent<NPCBrain>().RestartCurrentBehavior();

            // Stop being kinematic
            if(gameObject.GetComponent<Rigidbody>())
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        if(sprite && defaultMaterial)
            sprite.material = defaultMaterial;
    }
}
