using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleIndependentlyOfParent : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(1f, 1f, 1f);

    private Transform par;

    // Start is called before the first frame update
    void Start()
    {
        par = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = transform.localScale.WithX(targetScale.x / par.localScale.x).WithY(targetScale.y / par.localScale.y).WithZ(targetScale.z / par.localScale.z);
    }
}
