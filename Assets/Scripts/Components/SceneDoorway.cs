using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoorway : MonoBehaviour
{
    [Tooltip("The file name or path of the scene to unload.")]
    [SerializeField]
    private string oldSceneName;
    [Tooltip("The file name or path of the scene to load.")]
    [SerializeField]
    private string newSceneName;
    [Tooltip("The tag of the entity that is able to trigger this doorway to load the new scene.")]
    [SerializeField]
    private string userTag = "Player";

    

    private void OnTriggerEnter(Collider other) {
        // Only activate for game objects with the specified user tag
        if(other.CompareTag(userTag)){
            SceneManager.LoadScene(newSceneName, LoadSceneMode.Single);

            // Left for potential later use
            //StartCoroutine(SetNewActiveUnloadOld());
        }
    }

    // Left for potential later use
    IEnumerator SetNewActiveUnloadOld(){
        // Wait one frame to allow new scene to finish loading
        yield return 0;

        // Set new scene active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newSceneName));

        // Unload old scene
        SceneManager.UnloadSceneAsync(oldSceneName);
    }
}
