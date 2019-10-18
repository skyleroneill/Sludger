using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityManager : MonoBehaviour
{
    public bool debug = false;
    [SerializeField]
    private int maxAbilityPoints = 5;
    [SerializeField]
    private int currentAbilityPoints;
    [SerializeField]
    private Slider APBar;
    [SerializeField]
    private Text meterText;
    [SerializeField]
    private Image selectionImage;
    [SerializeField]
    private Image leftArrow;
    [SerializeField]
    private Image rightArrow;
    [SerializeField]
    private GameObject defaultButton;
    [SerializeField]
    private GameObject buttonContainer;
    [SerializeField]
    private Text[] slottedAbilityLabels;
    [SerializeField]
    private List<PlayerAbility> possibleAbilities;
    [SerializeField]
    private List<GameObject> abilityButtons = new List<GameObject>();

    // Default of -1 means no ability is selected
    private int selectedAbility = -1;

    private AbilitySlots abilitySlots;

    private void Start()
    {
        // Find the player's ability slots
        abilitySlots =  GameObject.FindGameObjectWithTag("Player").GetComponent<AbilitySlots>();

        // Initialize ability points and AP bar
        currentAbilityPoints = maxAbilityPoints;
        APBar.maxValue = maxAbilityPoints;
        APBar.value = currentAbilityPoints;

        // Initial set up of ability buttons
        for(int i = 0; i < possibleAbilities.Count; i++){
            // Ignore posiible abilities if they are not set
            if(possibleAbilities[i] == null)
                continue;

            // There are still buttons to be activated
            if(i < abilityButtons.Count){
                SetUpAbilityButton(i, possibleAbilities[i]);
            }
            // There are more abilities than buttons
            else{
                AddNewAbilityButton();
                SetUpAbilityButton(i, possibleAbilities[i]);
            }
        }
    }

    private void Update(){
        RefreshAPBar();
        ToggleScrollArrows();
        UpdateMeterText();
        MoveAndToggleSelectionImage();
    }

    // Click on a possible ability button
    public void SetSelectedAbility(int newAbil)
    {
        if (selectedAbility == newAbil)// Clicked on already selected ability, unselect it
            selectedAbility = -1;
        else if (newAbil >= 0 && newAbil <possibleAbilities.Count) // Newly selected ability is in range, select it
            selectedAbility = newAbil;
    }

    // Click an ability slot button
    public void ClickAbilitySlot(int slot)
    {
        if (selectedAbility == -1) // No selected ability, remove ability in the slot
            RemoveAbility(slot);
        else if (selectedAbility >= 0 && selectedAbility < possibleAbilities.Count) // Have an ability that is in range, add it
            AddAbility(slot);
    }

    public void AddNewAbilityOption(PlayerAbility abil){
        // Add new ability
        possibleAbilities.Add(abil);

        // Add its button
        // Ignore posiible abilities if they are not set
        if(possibleAbilities.Count-1 < abilityButtons.Count){
                SetUpAbilityButton(possibleAbilities.Count-1, abil);
            }
            // There are more abilities than buttons
            else{
                AddNewAbilityButton();
                SetUpAbilityButton(possibleAbilities.Count-1, abil);
            }
    }

    private void RefreshAPBar(){
        // If max ability points changes then update current ability points and APBar
        if(maxAbilityPoints != APBar.maxValue){
            currentAbilityPoints += maxAbilityPoints - (int)APBar.maxValue;
            APBar.maxValue = maxAbilityPoints;
        }

        // If AP bar's value doesn't match current ability points then set it to do so
        if(currentAbilityPoints != APBar.value){
            APBar.value = currentAbilityPoints;
        }
    }

    private void ToggleScrollArrows(){
        // Left arrow exists
        if(leftArrow){
            // Show arrow
            if(buttonContainer.GetComponent<RectTransform>().anchoredPosition.x < 0)
                leftArrow.enabled = true;
            else //  Hide arrow
                leftArrow.enabled = false;
        }

        // Right arrow exists
        if(rightArrow){ 
            // Show arrow
            if(buttonContainer.GetComponent<RectTransform>().anchoredPosition.x > (abilityButtons.Count - 9) * -200)
                rightArrow.enabled = true;
            else // Hide arrow
                rightArrow.enabled = false;
        }  
    }

    private void UpdateMeterText(){
        // No meter text, so get outta here!
        if(meterText == null)
            return;

        // Update the text
        meterText.text = currentAbilityPoints + "/" + maxAbilityPoints;
    }

    private void MoveAndToggleSelectionImage(){
        // No selection image. ABORT!
        if(selectionImage == null)
            return;

        // No ability selected
        if(selectedAbility == -1)
            selectionImage.enabled = false;
        else{ // Ability selected, move to its button position
            selectionImage.enabled = true;
            selectionImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(abilityButtons[selectedAbility].GetComponent<RectTransform>().anchoredPosition.x - 10f, 0f);
        }
    }

    private void AddAbility(int slot)
    {   
        // Get the slot the selected ability is already equipped in
        int equippedSlot = abilitySlots.GetEquippedSlot(possibleAbilities[selectedAbility]);

        // We're trying to equip an ability into the slot is already so do nothing
        if (equippedSlot == slot)
            return;
        // The selected ability is already in a slot, so remove it from that slot before we add it to the new one
        else if (equippedSlot >= 0)
        {
            RemoveAbility(equippedSlot);
        }

        // Check if the ability is too expensive
        if (currentAbilityPoints - possibleAbilities[selectedAbility].GetCost() < 0)
            return;

        // If there is already an ability equipped in this slot then remove it first
        if (abilitySlots.CheckEquipped(slot))
            RemoveAbility(slot);

        // Subtract the cost of equipping from the current amount of ability points
        currentAbilityPoints -= abilitySlots.EquipAbility(slot, possibleAbilities[selectedAbility]);
        
        // Set the AP bar's value
        APBar.value = currentAbilityPoints;

        // If this is a valid abilty slot, then set its slot label to say the new ability's name
        if(slot >= 0 && slot < slottedAbilityLabels.Length && slottedAbilityLabels[slot])
            slottedAbilityLabels[slot].text = possibleAbilities[selectedAbility].GetAbilityName();

        // Reset selected ability
        selectedAbility = -1;
    }

    private void RemoveAbility(int slot)
    {
        currentAbilityPoints += abilitySlots.RemoveAbility(slot);

        // Set the AP bar's value
        APBar.value = currentAbilityPoints;

        // If this is a valid abilty slot, then set its slot label to say it is empty
        if(slot >= 0 && slot < slottedAbilityLabels.Length && slottedAbilityLabels[slot])
            slottedAbilityLabels[slot].text = "Empty Slot";
    }

    private void SetUpAbilityButton(int b, PlayerAbility abil){
        abilityButtons[b].transform.Find("Text").gameObject.GetComponent<Text>().text = abil.GetAbilityName();
        abilityButtons[b].transform.Find("CostText").gameObject.GetComponent<Text>().text = abil.GetCost() + " AP";
        abilityButtons[b].GetComponent<Button>().interactable = true;
        abilityButtons[b].GetComponent<Button>().onClick.AddListener(() => {SetSelectedAbility(b);});
    }

    private void AddNewAbilityButton(){
        // Create new button and add it to the list of buttons
        GameObject newButton = Instantiate(defaultButton, buttonContainer.transform);
        abilityButtons.Add(newButton);

        // Scale the button container to fit the new button
        RectTransform containerTransform = buttonContainer.GetComponent<RectTransform>();
        containerTransform.sizeDelta = new Vector2(containerTransform.sizeDelta.x + 200f, containerTransform.sizeDelta.y);


        RectTransform newButtonTransform = newButton.GetComponent<RectTransform>();
        newButtonTransform.anchoredPosition = new Vector2((abilityButtons.Count-1) * 200f + 20f, 0f);
    }
}
