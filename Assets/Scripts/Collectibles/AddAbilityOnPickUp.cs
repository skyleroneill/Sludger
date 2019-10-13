using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAbilityOnPickUp : MonoBehaviour
{
    [SerializeField]
    private PlayerAbility ability;
    [SerializeField]
    private bool destroyOnPickUp = false;
    [SerializeField]
    private string collectorTag = "Player";
    [SerializeField]
    private PlayerAbilityManager abilityManager;

    private void OnCollisionEnter(Collision other) {
        // We can't add a new ability if there is no manager or ability set
        if(abilityManager == null || ability == null)
            return;

        // Did we collide with the collector of this object?
        if(other.gameObject.CompareTag(collectorTag)){
            // Add the new ability as an option in the ability manager
            abilityManager.AddNewAbilityOption(ability);

            // Destroy this object
            if(destroyOnPickUp)
                Destroy(gameObject);
        }
    }
}
