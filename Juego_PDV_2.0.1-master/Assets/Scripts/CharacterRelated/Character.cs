using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;

    public Animator MyAnimator { get; set; }

    private Vector2 direction;

    protected Rigidbody2D myRigidBody;

    public bool IsAttacking { get; set; }

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

    public Transform MyTarget { get; set; }

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

    public bool IsAlive
    {
        get{
             return health.MyCurrentValue > 0;
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health.Initialize(initHealth, initHealth);
        myRigidBody = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
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
        if (IsAlive) {
            myRigidBody.velocity = Direction.normalized * Speed;
            //Normalizar = hacerlo unitario
        }


    }

    public void HandleLayers()
    {
        if (IsAlive)
        {
            if (!isUsingSword)
            {
                if (isMoving)
                {
                    AnimateMovement(Direction);

                    ActivateLayer("WalkLayer");
                    MyAnimator.SetFloat("x", Direction.x);
                    MyAnimator.SetFloat("y", Direction.y);
                }
                else if (IsAttacking)
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
                    MyAnimator.SetFloat("x", Direction.x);
                    MyAnimator.SetFloat("y", Direction.y);

                }
                else if (IsAttacking)
                {
                    ActivateLayer("SwordAttackLayer");
                }
                else
                {
                    ActivateLayer("SwordBasicLayer");
                }
            }
        }
        else
        {
            ActivateLayer("DeathLayer");
        }

        

        
    }

    public void AnimateMovement(Vector2 direction)
    {
        
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }



    public virtual void TakeDamage(float damage, Transform source)
    {
        
        health.MyCurrentValue -= damage;
        if (health.MyCurrentValue <= 0)
        {
            Direction = Vector2.zero;
            myRigidBody.velocity = Direction;
            MyAnimator.SetTrigger("die");
        }
    }

}
