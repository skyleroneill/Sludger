using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    private static PlayerSingleton singleton;

    private void Awake(){
        // Only let there be one instance of the player
        if(singleton == null){
            singleton = this;
        } else if(singleton != this){
            Destroy(gameObject);
        }
    }
}
