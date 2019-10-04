using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDImageHealth : MonoBehaviour
{
    [Tooltip("The image that will shrink based upon health levels.")]
    [SerializeField]
    private Image heartImage;
    [Tooltip("The rate at which the heart image scale will move from the new to the old.")]
    [Range(0f, 1f)]
    [SerializeField]
    private float interpolant = 0.75f;
    [Tooltip("Once within this distance to the target scale the heart image will snap to the target.")]
    [SerializeField]
    private float snapThreshold = 0.05f;
    [SerializeField]
    private int maxValue = 25;
    [SerializeField]
    private int currentValue = 25;

    private Vector3 baseScale;
    private Vector3 targetScale;

    private void Start()
    {
        baseScale = heartImage.rectTransform.localScale;
        targetScale = Vector3.one;
    }

    private void Update()
    {
        // Calculate the new scale
        targetScale = baseScale * ((float)currentValue / (float)maxValue);

        // Interpolate to the new scale
        heartImage.rectTransform.localScale = Vector3.Lerp(heartImage.rectTransform.localScale, targetScale, interpolant);

        // Snap to the target scale if within the threshold
        if (Mathf.Abs(heartImage.rectTransform.localScale.magnitude - targetScale.magnitude) < snapThreshold)
            heartImage.rectTransform.localScale = targetScale;
    }
}
