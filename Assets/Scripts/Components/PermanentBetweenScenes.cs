using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentBetweenScenes : MonoBehaviour
{
    void Start()
    {
        // Don't destroy this game object when changing between scenes
        DontDestroyOnLoad(gameObject);
    }
}
