using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject youDied;

    private NPC currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        youDied.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //SEGUNDA ENTREGA SE VA A BORRAR
        if (player.MyHealth.MyCurrentValue <= 0)
        {
            Player.MyInstance.transform.position = new Vector3(-100, 0, 0);
            youDied.SetActive(true);
            youDied.GetComponent<Image>().color = Color.white;
        }


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
                    if (player.SpellType != null)
                    {
                        player.CastSpell(player.SpellType);
                    }
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
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
            if (hit.collider != null && hit.collider.tag == "Enemy")
            {
                currentTarget = hit.collider.GetComponent<NPC>();
                if (!currentTarget.IsAlive)
                {
                    foreach (Item item in currentTarget.items)
                    {
                        InventoryScript.MyInstance.AddItem(item);
                    }
                    player.TakeXP(currentTarget.MyXpPoints);
                    currentTarget.OnCharacterRemoved();
                }
            }
        }


    }
}
