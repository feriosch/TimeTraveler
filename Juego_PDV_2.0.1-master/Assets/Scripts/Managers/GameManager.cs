using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private NPC currentTarget;

    [SerializeField]
    private Item potion;
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
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) //Left click
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
            //En hit se guarda la ubicacion que cliqueo el usuario
            //Layer 9 = 2^9 = 512
            //En la capa 9 se guardan los enemigos (o todo lo que pueda recibir click izquierdo)
            if (hit.collider != null) //Toco algo
            {

                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }


                currentTarget = hit.collider.GetComponent<NPC>();

                player.MyTarget = currentTarget.Select();

                UIManager.MyInstance.ShowTargetFrame(currentTarget);
            }
            else //Deselect
            {
                UIManager.MyInstance.HideTargetFrame();

                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }

                currentTarget = null;
                player.MyTarget = null;
            }
        }
        else if (Input.GetMouseButtonDown(2) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
            if (hit.collider != null && hit.collider.tag == "Enemy")
            {
                currentTarget = hit.collider.GetComponent<NPC>();
                if (!currentTarget.IsAlive)
                {
                    HealthPotion potionInstance = (HealthPotion)Instantiate(potion);
                    InventoryScript.MyInstance.AddItem(potionInstance);
                    currentTarget.OnCharacterRemoved();
                }
            }
        }


    }
}
