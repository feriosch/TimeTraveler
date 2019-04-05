using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : ScriptableObject, IMovable //Script without GameObject
{
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private int stackSize;
    [SerializeField]
    private TextMeshProUGUI title;

    private SlotScript slot;

    public Sprite MyIcon { get => icon; }
    public int MyStackSize { get => stackSize; }
    public SlotScript MySlot { get => slot; set => slot = value; }
    public TextMeshProUGUI MyTitle { get => title; set => title = value; }

    public void Remove()
    {
        if(MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
}
