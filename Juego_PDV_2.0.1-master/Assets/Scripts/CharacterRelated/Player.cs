using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //[SerializeField]
    //private Stat health;
    //[SerializeField]
    //private int AttackType = 0;
    public int AttackType { get; set; }
    //[SerializeField]
    //private GameObject[] spellPrefab;
    private int exitIndex;
    [SerializeField]
    private Block[] blocks;
    private SpellBook spellBook;
    private Vector3 min, max;

    // Start is called before the first frame update
    protected override void Start()
    {
        spellBook = GetComponent<SpellBook>();
        //target = GameObject.Find("Target").transform;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
        base.Update();
    }

    public void GetInput()
    {
        Direction = Vector2.zero;

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
            Direction = Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            exitIndex = 2;
            Direction = Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 1;
            Direction = Vector2.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            Direction = Vector2.left;
        }

        //seleccion de item de ataque
        if (Input.GetKey(KeyCode.Alpha1))
        {
            isUsingSword = false;
            AttackType = 0;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            isUsingSword = true;
            AttackType = 1;
        }

        //ATAQUE PRINCIPAL CLICK DERECHO
        if (Input.GetMouseButtonDown(1))
        {
            CastSpell(AttackType);
        }

        if (isMoving)
        {
            StopAttack();
        }
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    private IEnumerator Attack(int attackTypeIndex)
    {
        Spell newSpell = spellBook.CastSpell(attackTypeIndex);
        IsAttacking = true;
        MyAnimator.SetBool("attack", IsAttacking);
        yield return new WaitForSeconds(newSpell.MyCastTime); //hardcode
        SpellScript s = Instantiate(newSpell.MySpellPrefab, transform.position, Quaternion.identity).GetComponent<SpellScript>();
        s.Initialize(MyTarget, transform);
        StopAttack();
    }

    public void CastSpell(int attackTypeIndex)
    {
        Block();
        //Clickeaste algo clickeable, no estas haciendo un ataque, no te estas moviendo y no tocas los collliders de la vista
        //El enemigo a atacar esta vivo
        //Atacar moviendote arruina las animaciones
        //Igualmente la animacion de un ataque debe terminar antes de empezar otro
        //Los colliders le dan mas realismo al juego
        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !IsAttacking && !isMoving && inLineOfSight())
        {
            attackRoutine = StartCoroutine(Attack(attackTypeIndex));
        }
    }

    private bool inLineOfSight()
    {
        if (MyTarget != null)
        {
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;
            //Debug.DrawRay(transform.position, targetDirection, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);
            if (hit.collider == null)
            {
                return true;
            }
            
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
        AttackType = attackTypeIndex;
    }

    public void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            IsAttacking = false;
            MyAnimator.SetBool("attack", IsAttacking);
        }
    }
}
