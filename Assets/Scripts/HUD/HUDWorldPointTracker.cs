using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDWorldPointTracker : MonoBehaviour
{
    public bool debug = false;
    [Tooltip("The world point this HUD element will track.")]
    public Transform trackPoint;

    private void Update()
    {
        if (!trackPoint)
        {
            if (debug) Debug.Log("No trackPoint set on: " + gameObject.name);
            return;
        }

        // Update the HUD element to be located at the screen position of the track point
        transform.position = Camera.main.WorldToScreenPoint(trackPoint.position);
    }
}
