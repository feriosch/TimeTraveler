using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public override void Exit()
    {
        
    }

    public override void Enter(Enemy parent)
    {
        base.Enter(parent);
        this.parent.Reset();
    }

    public override void Update()
    {
        //Debug.Log("IdleState");
        if (parent.MyTarget != null)
        {
            parent.ChangeState(new FollowState());
        }
    }
}
