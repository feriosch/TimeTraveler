using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : State
{
    public override void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    public override void Update()
    {
        //Debug.Log("FollowState");
        if (parent.MyTarget != null)
        {
            parent.Direction = (parent.MyTarget.transform.position - parent.transform.position).normalized;
            parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.MyTarget.position, parent.Speed * Time.deltaTime);

            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);
            if (distance <= parent.MyAttackRange)
            {
                parent.ChangeState(new AttackState());
            }
        }
        if (!parent.InRange)
        {
            parent.ChangeState(new EvadeState());
        }


    }
}
