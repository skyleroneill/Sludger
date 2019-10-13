using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/InteractAbility")]
public class InteractAbility : PlayerAbility
{
    public static InteractAbility interact;
    public delegate void Action();
    public event Action onInteractPressed;

    public override void Initialize(){
        interact = this;

        this.onInteractPressed += TestOnInteractEvent;
    }

    public override void ActivateAbility(){
        onInteractPressed();
    }

    private void TestOnInteractEvent(){
        if(debug) Debug.Log("Interact event triggered");
    }
}
