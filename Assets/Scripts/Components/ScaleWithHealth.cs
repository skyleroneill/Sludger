using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class ScaleWithHealth : MonoBehaviour
{
    [Tooltip("The rate at which the scale will move from the new to the old.")]
    [Range (0f, 1f)]
    [SerializeField]
    private float interpolant = 0.75f;
    [Tooltip("Once within this distance to the target scale this object will snap to the target.")]
    [SerializeField]
    private float snapThreshold = 0.05f;

    private Vector3 baseScale;
    private Vector3 targetScale;
    private Health hp;
    private CapsuleCollider c;

    private void Start(){
        baseScale = transform.localScale;
        targetScale = Vector3.one;
        hp = gameObject.GetComponent<Health>();
        c = gameObject.GetComponent<CapsuleCollider>();
    }

    private void Update(){
        // Calculate the new scale
        targetScale = baseScale * ((float)hp.GetHealth() / (float)hp.GetMaxHealth());

        // Ensure collider remains the same height
        c.height = 1f / targetScale.y;

        // Ensure that the collider remains above the ground
        c.center = c.center.WithY(c.height / 2f);

        // Interpolate to the new scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, interpolant);
        
        // Snap to the target scale if close
        if(Mathf.Abs(transform.localScale.magnitude - targetScale.magnitude) < snapThreshold)
            transform.localScale = targetScale;
    }
}
