﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();

public class NPC : Character
{
    public event HealthChanged healthChanged;

    public event CharacterRemoved characterRemoved;

    [SerializeField]
    private Sprite portrait;

    [SerializeField]
    private float xpPoints;

    [SerializeField]
    public Item[] items;

    public Sprite MyPortrait { get => portrait;}
    public float MyXpPoints { get => xpPoints; set => xpPoints = value; }

    public virtual void DeSelect()
    {
        healthChanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);
        characterRemoved -= new CharacterRemoved(UIManager.MyInstance.HideTargetFrame);
    }

    public virtual Transform Select() //virtual es para sobreescribir despues
    {
        return hitBox;
    }

    public void OnHealthChanged(float health)
    {
        //Antes de los eventos, hay que verificar que no tengan referencia nula
        if (healthChanged != null)
        {
            healthChanged(health);
        }
    }

    public void OnCharacterRemoved()
    {
        if (characterRemoved != null)
        {
            characterRemoved();
        }
        Destroy(gameObject);
    }
}
