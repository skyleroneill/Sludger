using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSingleton : MonoBehaviour
{
    private static CameraSingleton singleton;

    private void Awake(){
        // Only let there be one instance of the camera
        if(singleton == null){
            singleton = this;
        } else if(singleton != this){
            Destroy(gameObject);
        }
    }
}
