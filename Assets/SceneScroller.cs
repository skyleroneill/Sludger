using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScroller : MonoBehaviour
{
    private void Start(){
        DontDestroyOnLoad(gameObject);
    }

    private void Update(){
        // Load the next scene
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        // Load the previous scene
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
