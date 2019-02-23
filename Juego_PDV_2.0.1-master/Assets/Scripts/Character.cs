using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    protected Animator myAnimator;

    protected Vector2 direction;

    protected Rigidbody2D myRigidBody;

    protected bool isAttacking = false;

    protected Coroutine attackRoutine;

    //VARIABLES DE ANIMACION DE JUGADOR
    protected bool isUsingSword = false;
    protected bool isUsingGrayArmor = false;
    //VARIABLES DE ANIMACION DE JUGADOR

    public bool isMoving 
    {
        get {
            return direction.x != 0 || direction.y != 0;
        }
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {
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
        myRigidBody.velocity = direction.normalized * speed;
        //Normalizar = hacerlo unitario
    }

    public void HandleLayers()
    {
        if (!isUsingSword)
        {
            if (isMoving)
            {
                AnimateMovement(direction);

                ActivateLayer("WalkLayer");
                myAnimator.SetFloat("x", direction.x);
                myAnimator.SetFloat("y", direction.y);

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
                AnimateMovement(direction);

                ActivateLayer("SwordWalkLayer");
                myAnimator.SetFloat("x", direction.x);
                myAnimator.SetFloat("y", direction.y);

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
}
