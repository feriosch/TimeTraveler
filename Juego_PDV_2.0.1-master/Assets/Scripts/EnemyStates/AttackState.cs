using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    private float attackCooldown = .3f;
    private float extraRange = .1f;

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        //Debug.Log("AttackState");
        if (parent.MyAttackTime >= attackCooldown && !parent.IsAttacking)
        {
            parent.MyAttackTime = 0;
            parent.StartCoroutine(Attack());
        }
        if (parent.MyTarget != null)
        {
            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);
            if (distance >= parent.MyAttackRange + extraRange && !parent.IsAttacking)
            {
                parent.ChangeState(new FollowState());
            }
            //Checar rango y ataque
        }
        else
        {
            parent.ChangeState(new IdleState());
        }
    }

    public IEnumerator Attack()
    {
        parent.IsAttacking = true;

        parent.MyAnimator.SetTrigger("attack");

        Player.MyInstance.MyHealth.MyCurrentValue -= parent.MyAttackStrength;

        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length);

        //Player.MyInstance.MyHealth.MyCurrentValue -= parent.MyAttackStrength;

        parent.IsAttacking = false;
    }
}
