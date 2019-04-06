using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LootButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TextMeshProUGUI title;

    public Image MyIcon { get => icon; set => icon = value; }
    public TextMeshProUGUI MyTitle { get => title; set => title = value; }

    public Item MyLoot { get; set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryScript.MyInstance.AddItem(MyLoot))
        {
            gameObject.SetActive(false);
        }
        
    }
}
