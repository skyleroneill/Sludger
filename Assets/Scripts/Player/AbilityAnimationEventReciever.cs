using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAnimationEventReciever : MonoBehaviour
{
    [Tooltip("The ability slots component for which this script is recieveing animation events. Defaults to using the component in this object's parent.")]
    [SerializeField]
    private AbilitySlots abilitySlots;

    private void Start(){
        if(!abilitySlots)
            abilitySlots = transform.parent.gameObject.GetComponent<AbilitySlots>();
    }

    public void StartAbilityAnimationEvent(int ability){
        abilitySlots.StartAbilityAnimationEvent(ability);
    }

    public void EndAbilityAnimationEvent(int ability){
        abilitySlots.EndAbilityAnimationEvent(ability);
        
    }
}
