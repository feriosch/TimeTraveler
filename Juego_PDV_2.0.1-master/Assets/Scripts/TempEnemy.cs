using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : MonoBehaviour
{
    public float life = 100;

    public Sprite armor;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }
    // Update is called once per frame
    void Update()
    {
        HacerDano();
    }
    void OnGUI()
    {
        //GUILayout.BeginArea();
        GUILayout.Label("Enemy life: " + life);
    }
    public void HacerDano()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DealDamage(10);
        }

        if (life <= 0)
        {
            spriteRenderer.sprite = armor;
            //Destroy(this.gameObject);
        }
    }
    public void DealDamage(float amount)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
        life -= amount;
    }
}
