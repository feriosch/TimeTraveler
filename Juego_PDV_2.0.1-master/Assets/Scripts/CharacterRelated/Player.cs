using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    private static Player instance;
    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
    }

    [SerializeField]
    private int level;
    public int MyLevel { get => level; set => level = value; }


    public int AttackType { get; set; }
    public string SpellType { get; set; }
    private int exitIndex;
    [SerializeField]
    private Block[] blocks;

    private Vector3 min, max;

    [SerializeField]
    protected XP xp;
    public XP MyXP
    {
        get { return xp; }
    }

    

    [SerializeField]
    private float initXP;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        xp.Initialize(initXP, 100);
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        base.Update();
    }

    public void GetInput()
    {
        Direction = Vector2.zero;
        Speed = 4;

        //Debug
        if (Input.GetKeyDown(KeyCode.M))
        {
            TakeXP(10);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            health.MyCurrentValue += 10;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftAlt))
        {
            Speed = 6;
        }

        //movimiento del jugador
        if (IsAlive)
        {
            if (Input.GetKey(KeybindManager.MyInstance.KeyBinds["UP"]))
            {
                exitIndex = 0;
                Direction += Vector2.up;
            }
            if (Input.GetKey(KeybindManager.MyInstance.KeyBinds["DOWN"]))
            {
                exitIndex = 2;
                Direction += Vector2.down;
            }
            if (Input.GetKey(KeybindManager.MyInstance.KeyBinds["RIGHT"]))
            {
                exitIndex = 1;
                Direction += Vector2.right;
            }
            if (Input.GetKey(KeybindManager.MyInstance.KeyBinds["LEFT"]))
            {
                exitIndex = 3;
                Direction += Vector2.left;
            }
        }
        
       

        //seleccion de item de ataque
        /*if (Input.GetKey(KeyCode.Alpha1))
        {
            isUsingSword = false;
            //SpellType = "Punch"; 
            //AttackType = 0;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            isUsingSword = true;
            //SpellType = "Sword";
            //AttackType = 1;
        }*/

        //ATAQUE PRINCIPAL CLICK DERECHO
        if (Input.GetMouseButtonDown(1))
        {
            if (SpellType != null)
            {
                CastSpell(SpellType);
            }
            
        }

        if (isMoving)
        {
            StopAttack();
        }

        foreach (string action in KeybindManager.MyInstance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action]))
            {
                UIManager.MyInstance.ClickActionButton(action);
            }
        }
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    private IEnumerator Attack(string spellName)
    {
        Spell newSpell = SpellBook.MyInstance.CastSpell(spellName);
        //Debug.Log(spellName);
        IsAttacking = true;
        MyAnimator.SetBool("attack", IsAttacking);
        yield return new WaitForSeconds(newSpell.MyCastTime); //hardcode
        SpellScript s = Instantiate(newSpell.MySpellPrefab, transform.position, Quaternion.identity).GetComponent<SpellScript>();
        s.Initialize(MyTarget, transform);
        StopAttack();
    }

    public void CastSpell(string spellName)
    {
        Block();
        //Clickeaste algo clickeable, no estas haciendo un ataque, no te estas moviendo y no tocas los collliders de la vista
        //El enemigo a atacar esta vivo
        //Atacar moviendote arruina las animaciones
        //Igualmente la animacion de un ataque debe terminar antes de empezar otro
        //Los colliders le dan mas realismo al juego
        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !IsAttacking && !isMoving && inLineOfSight())
        {
            attackRoutine = StartCoroutine(Attack(spellName));
        }
    }

    private bool inLineOfSight()
    {
        if (MyTarget != null)
        {
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;
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

    public void TakeXP(float experience)
    {
        xp.MyCurrentValue += experience;
    }
}
