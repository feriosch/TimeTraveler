﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bag", menuName ="Items/Bag", order = 1)] //menu para el script fuera del objeto
public class Bag : Item, IUsable
{

    private int slots;

    [SerializeField]
    private GameObject bagPrefab;

    public BagScript MyBagScript { get; set; }
    public int Slots { get => slots; }

    public void Initialize(int slots)
    {
        this.slots = slots;
    }

    public void Use()
    {
        if (InventoryScript.MyInstance.CanAddBag)
        {
            Remove();
            MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();
            MyBagScript.AddSlots(slots);

            InventoryScript.MyInstance.AddBag(this);
        }
        
    }
}
