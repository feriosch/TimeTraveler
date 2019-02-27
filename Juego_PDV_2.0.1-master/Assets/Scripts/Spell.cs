using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    private Rigidbody2D myRigidBody;


    [SerializeField]
    private float speed;

    public Transform MyTarget { get; set; }

    //***
    //public TempEnemy enemy = new TempEnemy();

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        //TEST
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (MyTarget != null)
        {
            Vector2 direction = MyTarget.position - transform.position;
            myRigidBody.velocity = direction.normalized * speed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
       
    }

    /*private void OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            enemy.DealDamage(10);
        }

    }*/


    //*****
   
}
