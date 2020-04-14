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
    [SerializeField]
    private bool canUseAbilities = true;
    private const int SIZE = 8; // There can only be 8 abilities
    [Tooltip("The abilities this player has equipped. Up to four abilities can be equipped.\n0: Right Bumper\n1: Left Bumper\n2: Right Trigger\n3: Left Trigger\n4: Bottom Button\n5: Right Button\n6: Left Button\n7: Top Button")]
    [SerializeField]
    private Ability[] abilities = new Ability[SIZE];
    [Tooltip("Optional. If set then the player will use abilities according to the given input mode template.")]
    [SerializeField]
    private PlayerInputMode inputMode;

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
        // Can't use abilities while on a global cooldown or if the player can't use abilities
        if(onGlobalCooldown || !canUseAbilities) return;

        // Right bumper
        if(abilities[0].abil && !abilities[0].onCooldown && CheckAbilityInput(0))
        {
            abilities[0].abil.ActivateAbility();
            onGlobalCooldown = abilities[0].exclusiveUse;
            StartCoroutine(CooldownTime(0));
        }

        // Left bumber
        if(abilities[1].abil && !abilities[1].onCooldown && CheckAbilityInput(1))
        {
            abilities[1].abil.ActivateAbility();
            onGlobalCooldown = abilities[1].exclusiveUse;
            StartCoroutine(CooldownTime(1));
        }

        // Right trigger
        if(abilities[2].abil && !abilities[2].onCooldown && CheckAbilityInput(2))
        {
            abilities[2].abil.ActivateAbility();
            onGlobalCooldown = abilities[2].exclusiveUse;
            StartCoroutine(CooldownTime(2));
        }

        // Left trigger
        if(abilities[3].abil && !abilities[3].onCooldown && CheckAbilityInput(3))
        {
            abilities[3].abil.ActivateAbility();
            onGlobalCooldown = abilities[3].exclusiveUse;
            StartCoroutine(CooldownTime(3));
        }

        // Bottom Button, A/X
        if (abilities[4].abil && !abilities[4].onCooldown && CheckAbilityInput(4))
        {
            abilities[4].abil.ActivateAbility();
            onGlobalCooldown = abilities[4].exclusiveUse;
            StartCoroutine(CooldownTime(4));
        }

        // Right Button, B/Circle
        if(abilities[5].abil && !abilities[5].onCooldown && CheckAbilityInput(5))
        {
            abilities[5].abil.ActivateAbility();
            onGlobalCooldown = abilities[5].exclusiveUse;
            StartCoroutine(CooldownTime(5));
        }

        // Left Button, X/Square
        if(abilities[6].abil && !abilities[6].onCooldown && CheckAbilityInput(6))
        {
            abilities[6].abil.ActivateAbility();
            onGlobalCooldown = abilities[6].exclusiveUse;
            StartCoroutine(CooldownTime(6));
        }

        // Top Button, Y/Triangle
        if(abilities[7].abil && !abilities[7].onCooldown && CheckAbilityInput(7))
        {
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

    public bool CheckEquipped(int slot){
        return abilities[slot].abil != null;
    }

    public int GetEquippedSlot(PlayerAbility ability)
    {
        for(int i = 0; i < abilities.Length; i++)
        {
            // If we found the the ability in slot i, return i
            if (abilities[i].abil == ability)
                return i;
        }

        // The ability wasn't found in the ability slots, so -1 is returned
        return -1;
    }

    public void SetCanUseAbilities(bool b){
        canUseAbilities = b;
    }

    // Put an ability given by slot number on cooldown for its cooldown time
    IEnumerator CooldownTime(int slot){
        abilities[slot].onCooldown = true;
        yield return new WaitForSeconds(abilities[slot].cooldownTime);
        abilities[slot].onCooldown = false;
        onGlobalCooldown = false;
    }

    private bool CheckAbilityInput(int ability)
    {
        if (inputMode)
        {
           return inputMode.GetAbilityKeyDown(ability);
        }
        else
        {
            // Right bumper
            if (ability == 0)
            {
                return Input.GetButtonDown("Ability0");
            }

            // Left bumber
            if (ability == 1)
            {
                return Input.GetButtonDown("Ability1");
            }

            // Right trigger
            if (ability == 2 && !RTInUse)
            {
                RTInUse = true;
                return Input.GetAxis("Ability2") != 0f;
            }
            else if (ability == 2 && RTInUse)
            {
                if (Input.GetAxis("Ability2") == 0f) RTInUse = false;
                return false;
            }

            // Left trigger
            if (ability == 3 && !LTInUse)
            {
                RTInUse = true;
                return Input.GetAxis("Ability3") != 0f;
            }
            else if (ability == 2 && LTInUse)
            {
                if (Input.GetAxis("Ability3") == 0f) LTInUse = false;
                return false;
            }

            // Bottom Button, A/X
            if (ability == 4 && Input.GetButtonDown("BottomButton"))
            {
                return Input.GetButtonDown("Ability4");
            }

            // Right Button, B/Circle
            if (ability == 5 && Input.GetButtonDown("RightButton"))
            {
                return Input.GetButtonDown("Ability5");
            }

            // Left Button, X/Square
            if (ability == 6 && Input.GetButtonDown("LeftButton"))
            {
                return Input.GetButtonDown("Ability6");
            }

            // Top Button, Y/Triangle
            if (ability == 7 && Input.GetButtonDown("TopButton"))
            {
                return Input.GetButtonDown("Ability7");
            }
        }

        // Given an ability out of bounds
        return false;
    }
}
