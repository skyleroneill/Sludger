using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTargeting : MonoBehaviour
{
    public bool debug = false;

    [Tooltip("The objects that this object can target.")]
    [SerializeField]
    private List<GameObject> targets;
    [Tooltip("The index of the current target. Index 0 is the primary target.")]
    [SerializeField]
    private int currentTarget = 0;
    [Tooltip("The tag of the default ")]
    [SerializeField]
    private string defaultTargetTag = "Player";

    private void Awake(){
        // Ensure that there is always atleast one target.
        // Set target based upon a given tag.
        if(targets.Count == 0 || !targets[0]){
            targets.Add(GameObject.FindWithTag(defaultTargetTag));
        }

        // If the given tag is not on any object, then target self.
        // This is a measure to prevent errors.
        if(!targets[0]){
            targets[0] = gameObject;
            if(debug) Debug.Log(gameObject.name + ": Given default target tag could not be found.");
        }
    }

    public GameObject GetCurrentTarget(){
        return targets[currentTarget];
    }

    public GameObject GetTarget(int i){
        return targets[i];
    }

    public void ChangeTarget(int i){
        // Ensure that new target index is within range
        if(i >= targets.Count){
            if(debug) Debug.Log("Invalid target index.");
            return;
        }
        currentTarget = i;
    }

    public void AddNewTarget(GameObject newTarget){
        targets.Add(newTarget);
    }

    public float DistanceToCurrentTarget(){
        return Vector3.Distance(transform.position.WithY(0f), targets[currentTarget].transform.position.WithY(0f));
    }

    public float DistanceToTarget(int i){
        // Ensure that new target index is within range
        if(i >= targets.Count){
            if(debug) Debug.Log("Invalid target index.");
            return -1f;
        }
        return Vector3.Distance(transform.position, targets[i].transform.position);
    }

    public int GetCurrentTargetIndex(){
        return currentTarget;
    }

    public int GetNumberOfTargets(){
        return targets.Count;
    }
}
