using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public bool debug = false;
    [Tooltip("The player is invincible when God Mode is true.")]
    [SerializeField]
    private bool godMode = false;
    [Tooltip("Maximum number of hit points until death.")]
    [SerializeField]
    private int maxHitPoints = 10;
    [Tooltip("Current number of hit points.")]
    [SerializeField]
    private int currentHitPoints;
    [Tooltip("Time in seconds until the player can be hurt again after taking damage.")]
    [SerializeField]
    private float iTime = 0.15f;

    private bool onITime = false;
    private bool onIFrames = false;

    private void Start()
    {
        currentHitPoints = maxHitPoints;
    }

    private void Update()
    {
        // Debug keys: Q to cause 1 damage, P to heal 1 HP
        if (debug && Input.GetKeyDown(KeyCode.Q)) TakeDamage(1);
        if (debug && Input.GetKeyDown(KeyCode.P)) Heal(1);

        // Don't have HP less than zero
        if(currentHitPoints < 0) currentHitPoints = 0;
    }

    public int TakeDamage(int amount)
    {
        if (godMode || onITime || onIFrames) return 0;

        if (debug) Debug.Log("Hurting " + gameObject.name + " " + amount + " damage.");
        currentHitPoints -= amount;

        return amount;
    }

    // Take damage and activate invincibility
    public int TakeDamageInvincibility(int amount)
    {
        if (godMode || onITime || onIFrames) return 0;

        if (debug) Debug.Log("Hurting " + gameObject.name + " " + amount + " damage.");
        currentHitPoints -= amount;

        StartCoroutine(InvincibilityTime());
        return amount;
    }

    public int Heal(int amount)
    {
        // Case where amount given is an invalid heal amount
        if (amount <= 0f)
        {
            if (debug) Debug.Log("Cannot heal for an amount <= 0.");
            return currentHitPoints;
        }

        if (debug) Debug.Log("Healing " + gameObject.name + " " + amount + " damage.");
        currentHitPoints += amount;

        // Ensure we are never above our max hit points 
        if (currentHitPoints > maxHitPoints) currentHitPoints = maxHitPoints;

        return currentHitPoints;
    }

    public int GetHealth()
    {
        return currentHitPoints;
    }

    public void SetMaxHealth(int newMax)
    {
        float frac = currentHitPoints / maxHitPoints;
        maxHitPoints = newMax;
        currentHitPoints = (int)(maxHitPoints * frac);
    }

    public void SetHealth(int newHealth)
    {
        currentHitPoints = newHealth;

        // Ensure we are never above our max hit points 
        if (currentHitPoints > maxHitPoints) currentHitPoints = maxHitPoints;
    }

    public int GetMaxHealth()
    {
        return maxHitPoints;
    }

    public void resetHealth(int percent)
    {
        //between 0 and 100%
        if (percent > 100)
            percent = 100;
        else if (percent < 0)
            percent = 0;

        currentHitPoints = maxHitPoints * percent / 100;
    }

    public void SetIFrames(bool newVal){
        onIFrames = newVal;
    }

    public void SetGodMode(bool newVal)
    {
        godMode = newVal;
        if(debug) Debug.Log(gameObject.name + " God Mode: " + newVal);
    }

    public bool IsGod()
    {
        return godMode;
    }

    IEnumerator InvincibilityTime(){
        onITime = true;
        yield return new WaitForSeconds(iTime);
        onITime = false;
    }
}
