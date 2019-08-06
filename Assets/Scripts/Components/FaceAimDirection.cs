using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAimDirection : MonoBehaviour
{
    private PlayerAim pa;

    private void Start(){
        if(transform.parent.gameObject.GetComponent<PlayerAim>())
            pa = transform.parent.gameObject.GetComponent<PlayerAim>();
    }

    private void Update(){
        if(!pa) return;

        transform.rotation = Quaternion.LookRotation(pa.GetAimDirection());
    }
}
