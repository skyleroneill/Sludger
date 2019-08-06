using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float followSpeed = 2f;
    [SerializeField]
    private float maxDistance = 2f;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Vector3 angle;

    private void FixedUpdate(){
        FollowTarget();
    }

    private void FollowTarget(){
        Vector3 difference = target.position - (transform.position - offset);

        if(difference.magnitude > maxDistance){
            //transform.position = Vector3.Lerp(transform.position, transform.position + difference.normalized * (difference.magnitude - maxDistance) * followSpeed * Time.deltaTime, 0.95f);
            transform.position += difference.normalized * (difference.magnitude - maxDistance) * followSpeed * Time.deltaTime;
        }
    }

    public void SetTarget(Transform newTarget){
        target = newTarget;
    }

    public void SetTarget(GameObject newTarget){
        target = newTarget.transform;
    }
}
