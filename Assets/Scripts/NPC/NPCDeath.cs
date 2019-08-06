using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDeath : MonoBehaviour
{
    [SerializeField]
    private GameObject particles;
    [SerializeField]
    private GameObject[] powerUps;
    [SerializeField]
    [Range(0, 100)]
    private int powerUpDropChance = 10;

    private Health hp;

    private void Start(){
        hp = GetComponent<Health>();
    }

    private void Update(){
        if(hp.GetHealth() <= 0){
            // If one was specified, then spawn the particle system
            if(particles)
                Instantiate(particles, transform.position, transform.rotation);

            // Spawn random power ups if random number is within specified percent
            if(powerUps.Length > 1 && Random.Range(0, 101) <= powerUpDropChance){
                Instantiate(powerUps[Random.Range(0, powerUps.Length)], transform.position, transform.rotation);
            }else if(powerUps.Length == 1 && Random.Range(1, 101) <= powerUpDropChance)
                Instantiate(powerUps[0], transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
