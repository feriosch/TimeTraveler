using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private Stat health;

    private float initHealth = 100;

    [SerializeField]
    private int attackType = 0;

    [SerializeField]
    private GameObject[] spellPrefab;

    private int exitIndex;

    [SerializeField]
    private Block[] blocks;

    public Transform MyTarget { get; set; }


    // Start is called before the first frame update
    protected override void Start()
    {
        health.Initialize(initHealth, initHealth);
        //target = GameObject.Find("Target").transform;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();

        base.Update();
    }

    public void GetInput()
    {
        direction = Vector2.zero;

        //solamente para probar que funcione la vida
        if (Input.GetKeyDown(KeyCode.M))
        {
            health.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            health.MyCurrentValue += 10;
        }

        //movimiento del jugador
        if (Input.GetKey(KeyCode.W))
        {
            exitIndex = 0;
            direction = Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            exitIndex = 2;
            direction = Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 1;
            direction = Vector2.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            direction = Vector2.left;
        }

        //seleccion de item de ataque
        if (Input.GetKey(KeyCode.Alpha1))
        {
            isUsingSword = false;
            attackType = 0;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            isUsingSword = true;
            attackType = 1;
        }

        //ATAQUE PRINCIPAL
        if (Input.GetMouseButtonDown(1))
        {
            CastSpell(attackType);
        }

    }

    private IEnumerator Attack(int attackTypeIndex)
    {
        isAttacking = true;
        myAnimator.SetBool("attack", isAttacking);
        yield return new WaitForSeconds(0.4f); //hardcode

        Spell s = Instantiate(spellPrefab[attackTypeIndex], transform.position, Quaternion.identity).GetComponent<Spell>();
        s.MyTarget = MyTarget;
        StopAttack();
    }

    public void CastSpell(int attackTypeIndex)
    {
        Block();
        //Clickeaste algo clickeable, no estas haciendo un ataque, no te estas moviendo y no tocas los collliders de la vista
        //Atacar moviendote arruina las animaciones
        //Igualmente la animacion de un ataque debe terminar antes de empezar otro
        //Los colliders le dan mas realismo al juego
        if (MyTarget != null && !isAttacking && !isMoving && inLineOfSight())
        {
            attackRoutine = StartCoroutine(Attack(attackTypeIndex));
        }
    }

    private bool inLineOfSight()
    {
        Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

        //Debug.DrawRay(transform.position, targetDirection, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position),256);
        if (hit.collider == null)
        {
            return true;
        }
        return false;
    }

    private void Block()
    {
        foreach (Block b in blocks)
        {
            b.Deactivate();
        }
        blocks[exitIndex].Activate();
    }

    private void ChangeAttackType(int attackTypeIndex) //con este metodo cambia el ataque del HUD
    {
        attackType = attackTypeIndex;
    }
   
}
