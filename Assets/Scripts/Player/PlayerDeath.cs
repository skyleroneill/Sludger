using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class PlayerDeath : MonoBehaviour
{
    [SerializeField]
    private Text deathText;
    [SerializeField]
    private string deathMessage = "You would be dead.";

    private Health hp;

    private void Start()
    {
        hp = GetComponent<Health>();
        if (deathText == null) deathText = GameObject.FindGameObjectWithTag("DeathText").GetComponent<Text>();
    }

    void Update()
    {
        // Temporary hack
        if(hp.GetHealth() <= 0)
        {
            // Lock health at 1
            hp.SetHealth(1);

            // Display the death message
            if (deathText != null)
                deathText.text = deathMessage;
        }
        else if(hp.GetHealth() > 1 && deathText != null)
        {
            // Hide the death message
            deathText.text = "";
        }
    }
}
