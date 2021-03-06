﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField]
    private CanvasGroup healthGroup;

    [SerializeField]
    private float attackStrength;

    private State currentState;

    public float MyAttackRange { get; set; }

    public float MyAttackTime { get; set; }

    public Vector3 MyStartPosition { get; set; }

    [SerializeField]
    private float initAggroRange;

    public float MyAggroRange { get; set; }

    public bool InRange
    {
        get {
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
        }
    }

    public float MyAttackStrength { get => attackStrength; }

    protected void Awake()
    {
        MyStartPosition = transform.position;
        MyAggroRange = initAggroRange;
        MyAttackRange = 1.5f;
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        if(IsAlive)
        {
            if (!IsAttacking) //tiempo entre ataques
            {
                MyAttackTime += Time.deltaTime;
            }
            currentState.Update();
            
        }
        base.Update();

    }

    public override Transform Select()
    {
        healthGroup.alpha = 1;

        return base.Select();
    }

    public override void DeSelect()
    {
        healthGroup.alpha = 0;

        base.DeSelect();
    }

    public override void TakeDamage(float damage, Transform source)
    {
        if (!(currentState is EvadeState))
        {
            SetTarget(source);
            base.TakeDamage(damage, source);
            OnHealthChanged(health.MyCurrentValue);
        }
        
    }


    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;

        currentState.Enter(this);
    }

    public void SetTarget(Transform target)
    {
        if (MyTarget == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.position);
            MyAggroRange = initAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
        }
    }
     
    public void Reset()
    {
        this.MyTarget = null;
        this.MyAggroRange = initAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
        OnHealthChanged(health.MyCurrentValue);
        
    }
}
