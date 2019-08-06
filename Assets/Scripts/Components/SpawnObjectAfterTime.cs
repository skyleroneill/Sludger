using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectAfterTime : MonoBehaviour
{
    [Tooltip("Will this object destroy itself after spawning its object. True to destroy self.")]
    [SerializeField]
    private bool destroySelf = false;
    [Tooltip("The object to spawn after time is up.")]
    [SerializeField]
    private GameObject spawnObject;
    [Tooltip("The time in seconds until an item is spawned.")]
    [SerializeField]
    private float time = 1f;

    // Can an item be spawned
    private bool canSpawn = true;

    private void Start(){
        StartCoroutine(SpawnAfterTime());
    }

    private void Update(){
        if(canSpawn)
            StartCoroutine(SpawnAfterTime());
    }

    IEnumerator SpawnAfterTime(){
        canSpawn = false;
        yield return new WaitForSeconds(time);
        canSpawn = true;
        // Spawn the object
        Instantiate(spawnObject, transform.position, transform.rotation);
        // Destroy self if told to
        if (destroySelf) Destroy(gameObject);
    }
}
