using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI stackSize;

    public bool IsEmpty
    {
        get
        {
            return MyItems.Count == 0;
        }
    }

    public bool IsFull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }
            return true;
        }
    }

    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return MyItems.Peek();
            }
            return null;
        }
    }

    public Image MyIcon
    {
        get
        {
            return icon;
        }
        set
        {
            icon = value;
        }
    }

    public int MyCount
    {
        get
        {
            return MyItems.Count;
        }
    }

    public TextMeshProUGUI MyStackText => stackSize;

    public ObservableStack<Item> MyItems { get => items; set => items = value; }

    private void Awake()
    {
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public bool AddItem(Item item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;
            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }
                AddItem(newItems.Pop());
            }
            return true;
        }
        return false;
    }

    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
        }
    }

    public void Clear()
    {
        if (MyItems.Count > 0)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
            MyItems.Clear();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.MyInstance.FromSlot == null) //No tenemos item que mover
            { //Pickup Item
                HandScript.MyInstance.TakeMovable(MyItem as IMovable);
                InventoryScript.MyInstance.FromSlot = this;
            }
            else if (InventoryScript.MyInstance.FromSlot != null) //Si tenemos item que mover
            {
                //El orden del or es importante
                if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) || SwapItems(InventoryScript.MyInstance.FromSlot) || AddItems(InventoryScript.MyInstance.FromSlot.MyItems))
                {
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }
            }

        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (MyItem is IUsable)
        {
            (MyItem as IUsable).Use();
        }
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize)
        {
            MyItems.Push(item);
            item.MySlot = this;
            return true;
        }
        return false;
    }

    private bool PutItemBack()
    {
        if (InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            return true;
        }
        return false;
    }

    public bool SwapItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize)
        {
            //Copiar tdos los elementos a swapear desde A
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems);
            //Formateamos de A
            from.MyItems.Clear();
            //Tods los elementos de B se copian a A
            from.AddItems(MyItems);
            //Borramos B
            MyItems.Clear();
            //Movemos los elementos de la copia de A a B.
            AddItems(tmpFrom);

            return true;
        }
        return false;
    }

    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            //Cuantso espacios disponibles tenemos en el stack;
            int free = MyItem.MyStackSize - MyCount;
            for (int i = 0; i < free; i++)
            {
                AddItem(from.MyItems.Pop());
            }
            return true;
        }
        return false;
    }

    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }
}
