﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ItemCountChanged(Item item); //Misma estructura que la funcion

public class InventoryScript : MonoBehaviour
{
    public event ItemCountChanged itemCountChangedEvent;

    private static InventoryScript instance;

    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }
    }

    private SlotScript fromSlot;

    private List<Bag> bags = new List<Bag>();

    [SerializeField]
    private BagButton[] bagButtons;

    public bool CanAddBag
    {
        get { return bags.Count < 1; }
    }

    public SlotScript FromSlot
    {
        get
        {
            return fromSlot;
        }
        set
        {
            fromSlot = value;
            if (value != null)
            {
                fromSlot.MyIcon.color = Color.grey;
            }
        }
    }



    //DEBUGGING
    [SerializeField]
    private Item[] items;

    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(20);
        bag.Use();
    }
    //DEBUGGING

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(9);
            bag.Use();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[1]);
            AddItem(potion);
        }
    }



    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;
                bags.Add(bag);
                break;
            }
        }
    }

    public bool AddItem(Item item)
    {
        if (item.MyStackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return true;
            }
        }

        return PlaceInEmpty(item);
    }

    private bool PlaceInEmpty(Item item)
    {
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.AddItem(item))
            {
                OnItemCountChanged(item);
                return true;
            }
        }
        return false;
    }

    private bool PlaceInStack(Item item)
    {
        foreach (Bag bag in bags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (slot.StackItem(item))
                {
                    OnItemCountChanged(item);
                    return true;
                }
            }
        }
        return false;
    }

    public void OpenClose()
    {
        bool closedbBag = bags.Find(x => !x.MyBagScript.IsOpen);
        //Si la bolsa esta cerrada, entonces abrimos todas las bolsas
        //Si la bolsa no esta cerrada, entonces cerramos todas las bolsas
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.IsOpen != closedbBag)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }

    public Stack<IUsable> GetUsables(IUsable type)
    {
        Stack<IUsable> usables = new Stack<IUsable>();
        foreach (Bag bag in bags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.GetType() == type.GetType())
                {
                    foreach (Item item in slot.MyItems) 
                    {
                        usables.Push(item as IUsable);
                    }
                }
            }
        }

        return usables;
    }

    public void OnItemCountChanged(Item item)
    {
        if (itemCountChangedEvent != null) //Evitar NullReferenceException
        {
            itemCountChangedEvent.Invoke(item);
        }
    }
}
