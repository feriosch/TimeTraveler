using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(LayerMask.GetMask("Clicks"));
        ClickTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0)) //Left click
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
            //En hit se guarda la ubicacion que cliqueo el usuario
            //Layer 9 = 2^9 = 512
            //En la capa 9 se guardan los enemigos (o todo lo que pueda recibir click izquierdo)
            //if (hit.collider != null) //Toco algo
            
                //if (hit.collider.tag == "Enemy")
                
             player.MyTarget = hit.transform;
                
                
                //Debug.Log("Hola")
        }
        
    }
}
