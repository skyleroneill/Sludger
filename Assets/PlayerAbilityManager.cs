using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityManager : MonoBehaviour
{
    [SerializeField]
    private int abilityPoints = 5;
    [SerializeField]
    private int currentAbilityPoints;
    [SerializeField]
    private Slider APBar;
    [SerializeField]
    private PlayerAbility[] possibleAbilities;

    // Default of -1 means no ability is selected
    private int selectedAbility = -1;

    private AbilitySlots abilitySlots;

    private void Start()
    {
        // Find the player's ability slots
        abilitySlots =  GameObject.FindGameObjectWithTag("Player").GetComponent<AbilitySlots>();
        currentAbilityPoints = abilityPoints;
    }

    public void SetSelectedAbility(int newAbil)
    {
        if (selectedAbility == newAbil)// Clicked on already selected ability, unselect it
            selectedAbility = -1;
        else if (newAbil >= 0 && newAbil <possibleAbilities.Length) // Newly selected ability is in range, select it
            selectedAbility = newAbil;
    }

    public void ClickAbilitySlot(int slot)
    {
        if (selectedAbility == -1) // No selected ability, remove ability in the slot
            RemoveAbility(slot);
        else if (selectedAbility >= 0 && selectedAbility < possibleAbilities.Length) // Have an ability that is in range, add it
            AddAbility(slot);
    }

    private void AddAbility(int slot)
    {   
        // Check if the ability is already equipped or it is too expensive
        if (abilitySlots.CheckEquipped(possibleAbilities[selectedAbility]) && currentAbilityPoints - possibleAbilities[selectedAbility].GetCost() >= 0)
            return;

        // Subtract the cost of equipping from the current amount of ability points
        currentAbilityPoints -= abilitySlots.EquipAbility(slot, possibleAbilities[selectedAbility]);
        
        // Set the AP bar's value
        APBar.value = currentAbilityPoints;

        // Reset selected ability
        selectedAbility = -1;
    }

    private void RemoveAbility(int slot)
    {
        currentAbilityPoints += abilitySlots.RemoveAbility(slot);

        // Set the AP bar's value
        APBar.value = currentAbilityPoints;
    }
}
