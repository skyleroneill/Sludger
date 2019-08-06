﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSubState : StateMachineBehaviour
{
    public int layer = 0;

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        animator.SetInteger("substate", layer);
    }
}
