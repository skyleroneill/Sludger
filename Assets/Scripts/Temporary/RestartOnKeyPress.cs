using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartOnKeyPress : MonoBehaviour
{
    public KeyCode restartKey;

    private GameObject player;
    private Health hp;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        hp = player.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(restartKey))
        {
            player.transform.position = new Vector3(-13.28f, 0f, 0f);
            hp.Heal(420);
            SceneManager.LoadScene(0);
        }

    }
}
