using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHealthBubble : MonoBehaviour
{
    public bool debug = false;
    [Tooltip("The image that will shrink based upon health levels.")]
    [SerializeField]
    private Image heartImage;
    [SerializeField]
    private Text valueText;
    [Tooltip("The rate at which the heart image scale will move from the new to the old.")]
    [Range(0f, 1f)]
    [SerializeField]
    private float interpolant = 0.1f;
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

        // Ensure there is a heart image
        if (heartImage == null)
            heartImage = gameObject.GetComponentInChildren<Image>();

        // Ensure there is value text
        if (valueText == null)
            valueText = gameObject.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        // Don't do anything if we don't have a heart image or value text
        if (heartImage == null || valueText == null)
            return;

        valueText.text = "" + currentValue;

        // Calculate the new scale
        targetScale = baseScale * ((float)currentValue / (float)maxValue);

        // Interpolate to the new scale
        heartImage.rectTransform.localScale = Vector3.Lerp(heartImage.rectTransform.localScale, targetScale, interpolant);

        // Snap to the target scale if within the threshold
        if (Mathf.Abs(heartImage.rectTransform.localScale.magnitude - targetScale.magnitude) < snapThreshold)
            heartImage.rectTransform.localScale = targetScale;
    }

    public bool SetMaxvalue(int newMax)
    {
        // The new max value must be larger than zero
        if (newMax < 1)
        {
            if (debug) Debug.Log(newMax + " is not a valid new max value. It must be > 0.");
            return false;
        }

        maxValue = newMax;
        return true;
    }

    public bool SetCurrentvalue(int newVal)
    {
        // The new value must be larger than zero
        if (newVal < 1)
        {
            if (debug) Debug.Log(newVal + " is not a valid new value. It must be > 0.");
            return false;
        }

        currentValue = newVal;
        return true;
    }
}
