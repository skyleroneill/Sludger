using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class HealthHUDController : MonoBehaviour
{
    public bool debug = false;
    [Tooltip("The tag of the health bubble that this script will modify. Only used if healthBubble is not set.")]
    [SerializeField]
    private string healthBubbleTag = "HealthBubble";
    [Tooltip("The health bubble that this script will modify. If not set then will be set to the above tag.")]
    [SerializeField]
    private HUDHealthBubble healthBubble;

    private Health hp;

    private void Start()
    {
        hp = gameObject.GetComponent<Health>();

        // Set the health bubble via tag if no other health bubble was given
        if (healthBubble == null)
            healthBubble = GameObject.FindGameObjectWithTag(healthBubbleTag).GetComponent<HUDHealthBubble>();

        if (healthBubble == null && debug)
            Debug.Log("Setting health bubble according to tag failed."); 
    }

    private void Update()
    {
        // Set health bubble value
        healthBubble.SetCurrentvalue(hp.GetHealth());
    }
}
