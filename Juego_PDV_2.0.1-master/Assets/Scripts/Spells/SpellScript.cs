﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidBody;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float spellTime;

    private float time = 0;


    public Transform MyTarget { get; private set; }

    private Transform source;

    [SerializeField]
    private int damage = 10;

    //private bool alive = true; //El ataque sigue vivo, aun no se ha detenido. 

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        //TEST
        
    }

    public void Initialize(Transform target, Transform source)
    {
        MyTarget = target;
        this.source = source;
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

        time += Time.deltaTime;
        if (time > spellTime)
        {
            speed = 0;
            GetComponent<Animator>().SetTrigger("Impact");
            myRigidBody.velocity = Vector2.zero;
            time = 0;
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyHitbox" && collision.transform == MyTarget.transform)
        {
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            c.TakeDamage(damage, source);
            //collision.GetComponentInParent<Enemy>().TakeDamage(damage);
            GetComponent<Animator>().SetTrigger("Impact");
            myRigidBody.velocity = Vector2.zero;
            MyTarget = null;
        }
    }
}
