using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public override void Exit()
    {
        
    }

    public override void Update()
    {
        if (parent.Target != null)
        {
            parent.ChangeState(new FollowState());
        }
    }
}
