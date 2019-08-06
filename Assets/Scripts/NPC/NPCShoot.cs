using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCAim))]
public class NPCShoot : MonoBehaviour
{
    public bool debug = false;

    [Tooltip("The bullets that this NPC can fire.")]
    [SerializeField]
    private GameObject[] bullets;
    [Tooltip("The transforms from which bullets will be spawned.")]
    [SerializeField]
    private Transform[] bulletOrigins;
    [Tooltip("The time in seconds between a bullet being fired.")]
    [SerializeField]
    private float cooldown = 0.2f;
    [Tooltip("The time in seconds between a burst of bullets being fired. Places a cooldown on the firebullets function.")]
    [SerializeField]
    private float burstCooldown = 1f;
    [Tooltip("Whether or not bullets fired will hurt others on the same layer. True if bullets will hurt objects on the same layer. False if bullets will ignore objects on the same layer. Set to zero for rapid fire.")]
    [SerializeField]
    private bool friendlyFire = false;

    private int currBullet = 0;
    private int currOrigin = 0;
    private int loaded = 0;
    private bool canFire = true;
    private bool canBurst = true;
    private NPCAim na;

    private void Start(){
        na = GetComponent<NPCAim>();
    }

    private void Update(){
        // For Debugging only
        if(debug && Input.GetKeyDown(KeyCode.P)) FireBullets(2);

        // Ensure that the NPC is currently able to fire a burst
        if(canBurst){
            // While there are still more bullets to fire and this object is not on cooldown
            // Then fire a bullet and decrement the number of bullets left to fire
            if(loaded > 0 && canFire){
                Fire();
                loaded--;
            }
            // No more bullets loaded in this burst
            // Begin burst cooldown
            else if(loaded <= 0 && burstCooldown > 0){
                StartCoroutine(BurstCooldown());
            }
        }
    }

    private void Fire(){
        // Spawn and setup a bullet
        GameObject b;
        b = Instantiate(bullets[currBullet], bulletOrigins[currOrigin].position, Quaternion.Euler(na.GetAimDirection()));
        // Only setup the bullet if it is a projectile 
        if(b.GetComponent<Projectile>()){
            b.GetComponent<Projectile>().SetDirection(na.GetAimDirection());
            b.GetComponent<Projectile>().SetShooter(gameObject);

            if(!friendlyFire) b.GetComponent<Projectile>().AddIgnoreLayer(gameObject.layer);
        }

        // Start cooldown routine
        StartCoroutine(Cooldown());
    }

    public void FireBullets(int amount = 1, int bullet = 0, int origin = 0){
        // Only fire more bullets if not already firing a burst
        if(loaded > 0) return;

        // Ensure that the bullet, origin, and amount are valid
        if(bullet >= bullets.Length || origin >= bulletOrigins.Length) return;
        if(!bullets[bullet] || !bulletOrigins[origin]) return;
        if(amount <= 0) return;

        // Set the bullet firing variables
        currBullet = bullet;
        currOrigin = origin;
        loaded = amount;
    }

    public void SetCooldown(float cd){
        // Negative cooldowns are not valid
        if(cd < 0f) return;

        cooldown = cd;
    }

    public float GetCooldown(){
        return cooldown;
    }

    public void SetBurstCooldown(float cd){
        // Negative cooldowns are not valid
        if(cd < 0f) return;

        burstCooldown = cd;
    }

    public float GetBurstCooldown(){
        return burstCooldown;
    }

    public void StopBurst(){
        // Set amount loaded to zero, effectively ending the burst fire
        loaded = 0;
    }

    public void ResetCooldowns(){
        // Allow firing again
        canFire = true;
        canBurst = true;
    }

    public Transform GetFirePoint(int i){
        // Return the current firepoint if the given index is out of bounds
        if(i >= bulletOrigins.Length)
            return bulletOrigins[currOrigin];
        
        return bulletOrigins[i];
    }

    public int GetRemainingBullets(){
        return loaded;
    }

    IEnumerator Cooldown(){
        // Prevent shooting
        canFire = false;

        // Wait for number of seconds specified by coolDown
        yield return new WaitForSeconds(cooldown);

        // Reenable shooting
        canFire = true;
    }

    IEnumerator BurstCooldown(){
        // Prevent shooting
        canBurst = false;

        // Wait for number of seconds specified by coolDown
        yield return new WaitForSeconds(burstCooldown);

        // Reenable shooting
        canBurst = true;
    }
}
