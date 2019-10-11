using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Ability
{
    public PlayerAbility abil;
    [HideInInspector]
    public bool exclusiveUse;
    [HideInInspector]
    public bool onCooldown;
    [HideInInspector]
    public float cooldownTime;
}

public class AbilitySlots : MonoBehaviour
{
    private const int SIZE = 8; // There can only be 4 abilities
    [Tooltip("The abilities this player has equipped. Up to four abilities can be equipped.\n0: Right Bumper\n1: Left Bumper\n2: Right Trigger\n3: Left Trigger\n4: Bottom Button\n5: Right Button\n6: Left Button\n7: Top Button")]
    [SerializeField]
    private Ability[] abilities = new Ability[SIZE];

    // True if the used ability initiates a gloabl cooldown when used.
    // Used for abilities that cannot have other abilities used simultaneously.
    private bool onGlobalCooldown = false;

    private bool RTInUse = false;
    private bool LTInUse = false;

    private void Start(){
        // Initialize the player's abilities
        for (int i = 0; i < SIZE; i++)
        {
            if(abilities[i].abil == null) continue;
            abilities[i].abil.SetPlayer(gameObject);
            abilities[i].abil.Initialize();
            abilities[i].onCooldown = false;
            abilities[i].cooldownTime = abilities[i].abil.GetCooldown();
            abilities[i].exclusiveUse = abilities[i].abil.GetExclusiveUse();
        }
    }

    private void Update(){
        // Can't use abilities while on a global cooldown
        if(onGlobalCooldown) return;

        // Right bumper
        if(abilities[0].abil && !abilities[0].onCooldown && Input.GetButtonDown("Ability0")){
            abilities[0].abil.ActivateAbility();
            onGlobalCooldown = abilities[0].exclusiveUse;
            StartCoroutine(CooldownTime(0));
        }

        // Left bumber
        if(abilities[1].abil && !abilities[1].onCooldown && Input.GetButtonDown("Ability1")){
            abilities[1].abil.ActivateAbility();
            onGlobalCooldown = abilities[1].exclusiveUse;
            StartCoroutine(CooldownTime(1));
        }

        // Right trigger
        if(abilities[2].abil && !abilities[2].onCooldown && !RTInUse && (Input.GetAxis("Ability2") != 0f || Input.GetKeyDown(KeyCode.Space))){
            RTInUse = true;
            abilities[2].abil.ActivateAbility();
            onGlobalCooldown = abilities[2].exclusiveUse;
            StartCoroutine(CooldownTime(2));
        }
        // Right trigger no longer in use
        if (Input.GetAxis("Ability2") == 0f) RTInUse = false;

        // Left trigger
        if(abilities[3].abil && !abilities[3].onCooldown && !LTInUse && Input.GetAxis("Ability3") != 0f){
            LTInUse = true;
            abilities[3].abil.ActivateAbility();
            onGlobalCooldown = abilities[3].exclusiveUse;
            StartCoroutine(CooldownTime(3));
        }
        // Right trigger no longer in use
        if (Input.GetAxis("Ability3") == 0f) LTInUse = false;

        // Bottom Button, A/X
        if (abilities[4].abil && !abilities[4].onCooldown && ((Input.GetButtonDown("BottomButton")) || (Input.GetKeyDown(KeyCode.Return)))){
            abilities[4].abil.ActivateAbility();
            onGlobalCooldown = abilities[4].exclusiveUse;
            StartCoroutine(CooldownTime(4));
        }

        // Right Button, B/Circle
        if(abilities[5].abil && !abilities[5].onCooldown && Input.GetButtonDown("RightButton")){
            abilities[5].abil.ActivateAbility();
            onGlobalCooldown = abilities[5].exclusiveUse;
            StartCoroutine(CooldownTime(5));
        }

        // Left Button, X/Square
        if(abilities[6].abil && !abilities[6].onCooldown && Input.GetButtonDown("LeftButton")){
            abilities[6].abil.ActivateAbility();
            onGlobalCooldown = abilities[6].exclusiveUse;
            StartCoroutine(CooldownTime(6));
        }

        // Top Button, Y/Triangle
        if(abilities[7].abil && !abilities[7].onCooldown && Input.GetButtonDown("TopButton")){
            abilities[7].abil.ActivateAbility();
            onGlobalCooldown = abilities[7].exclusiveUse;
            StartCoroutine(CooldownTime(7));
        }
    }

    // Validate that the abilities array is never greater than 4
     void OnValidate(){
        if (abilities.Length != SIZE){
            Debug.LogWarning("Don't change the 'abilities' field's array size!");
            System.Array.Resize(ref abilities, SIZE);
        }
    }

    public void StartAbilityAnimationEvent(int a){
        // Ensure the given ability exists
        if(!abilities[a].abil || a >= SIZE) return;

        abilities[a].abil.StartAnimationEvent();
    }

    public void EndAbilityAnimationEvent(int a){
        // Ensure the given ability exists
        if(!abilities[a].abil || a >= SIZE) return;

        abilities[a].abil.EndAnimationEvent();
    }

    public int EquipAbility(int slot, PlayerAbility ability)
    {
        // Equip and set up the new ability
        abilities[slot].abil = ability;
        abilities[slot].abil.SetPlayer(gameObject);
        abilities[slot].abil.Initialize();
        abilities[slot].onCooldown = false;
        abilities[slot].cooldownTime = abilities[slot].abil.GetCooldown();
        abilities[slot].exclusiveUse = abilities[slot].abil.GetExclusiveUse();
        return ability.GetCost(); // Return the cost of the added ability
    }

    public int RemoveAbility(int slot)
    {
        // No ability, cost is 0
        if (abilities[slot].abil == null) return 0;

        int abilCost = abilities[slot].abil.GetCost();
        // Unset the ability by setting it to null
        abilities[slot].abil = null;
        return abilCost; // Return the cost of the removed ability
    }

    public bool CheckEquipped(PlayerAbility ability)
    {
        foreach (Ability a in abilities)
        {
            // If the ability is found in the ability slots then it is equipped
            if (a.abil == ability)
                return true;
        }
        // The abilty wasn't found in the ability slots, so it is not equipped
        return false;
    }

    // Put an ability given by slot number on cooldown for its cooldown time
    IEnumerator CooldownTime(int slot){
        abilities[slot].onCooldown = true;
        yield return new WaitForSeconds(abilities[slot].cooldownTime);
        abilities[slot].onCooldown = false;
        onGlobalCooldown = false;
    }
}
