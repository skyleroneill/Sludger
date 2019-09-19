using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnTouch : MonoBehaviour
{
    [SerializeField]
    private int amount = 25;
    [SerializeField]
    private string healTag = "Player";
    [SerializeField]
    private bool destroyAfterHealing = false;

    private void OnCollisionEnter(Collision other)
    {
        // Heal the gameobject we collided with if it is tagged with the heal tag and it has health
        if(other.gameObject.tag == healTag && other.gameObject.GetComponent<Health>())
        {
            other.gameObject.GetComponent<Health>().Heal(amount);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Heal the gameobject we collided with if it is tagged with the heal tag and it has health
        if (other.gameObject.tag == healTag && other.gameObject.GetComponent<Health>())
        {
            other.gameObject.GetComponent<Health>().Heal(amount);
        }
    }
}
