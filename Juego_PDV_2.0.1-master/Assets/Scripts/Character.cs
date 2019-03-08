using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;

    protected Animator myAnimator;

    private Vector2 direction;

    protected Rigidbody2D myRigidBody;

    protected bool isAttacking = false;

    protected Coroutine attackRoutine;

    [SerializeField]
    protected Transform hitBox;

    [SerializeField]
    protected Stat health;

    public Stat MyHealth
    {
        get { return health; }
    }

    [SerializeField]
    private float initHealth;

    //VARIABLES DE ANIMACION DE JUGADOR
    protected bool isUsingSword = false;
    protected bool isUsingGrayArmor = false;
    //VARIABLES DE ANIMACION DE JUGADOR

    public bool isMoving 
    {
        get {
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public Vector2 Direction { get => direction; set => direction = value; }
    public float Speed { get => speed; set => speed = value; }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        health.Initialize(initHealth, initHealth);
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        myRigidBody.velocity = Direction.normalized * Speed;
        //Normalizar = hacerlo unitario
    }

    public void HandleLayers()
    {
        if (!isUsingSword)
        {
            if (isMoving)
            {
                AnimateMovement(Direction);

                ActivateLayer("WalkLayer");
                myAnimator.SetFloat("x", Direction.x);
                myAnimator.SetFloat("y", Direction.y);

                StopAttack();
            }
            else if (isAttacking)
            {
                ActivateLayer("AttackLayer");
            }
            else
            {
                ActivateLayer("BasicLayer");
            }
        }

        if (isUsingSword)
        {
            if (isMoving)
            {
                AnimateMovement(Direction);

                ActivateLayer("SwordWalkLayer");
                myAnimator.SetFloat("x", Direction.x);
                myAnimator.SetFloat("y", Direction.y);

                StopAttack();
            }
            else if (isAttacking)
            {
                ActivateLayer("SwordAttackLayer");
            }
            else
            {
                ActivateLayer("SwordBasicLayer");
            }
        }

        
    }

    public void AnimateMovement(Vector2 direction)
    {
        
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < myAnimator.layerCount; i++)
        {
            myAnimator.SetLayerWeight(i, 0);
        }
        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName), 1);
    }

    public void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            isAttacking = false;
            myAnimator.SetBool("attack", isAttacking); 
        }
        
    }

    public virtual void TakeDamage(float damage)
    {
        health.MyCurrentValue -= damage;
        if (health.MyCurrentValue <= 0)
        {
            myAnimator.SetTrigger("Die");
        }
    }

}
