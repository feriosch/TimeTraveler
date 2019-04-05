using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LootWindow : MonoBehaviour
{
    [SerializeField]
    private LootButton[] lootButtons;


    //DEBUGGING
    [SerializeField]
    private Item[] items;
    // Start is called before the first frame update
    void Start()
    {
        AddLoot();
    }

    private void AddLoot()
    {
        //Poner el icono en los botones
        lootButtons[0].MyIcon.sprite = items[0].MyIcon;
        //Asegurarnos de que es visible
        lootButtons[0].gameObject.SetActive(true);

        TextMeshProUGUI title = items[0].MyTitle;
        //Asegurar que el titulo sea correcto
        lootButtons[0].MyTitle = title;

        lootButtons[0].MyLoot = items[0];
    }
}
