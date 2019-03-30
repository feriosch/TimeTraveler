using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class Spell : IUsable
{
    [SerializeField]
    private String name;
    [SerializeField]
    private int damage;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float castTime;
    [SerializeField]
    private GameObject spellPrefab;

    public string MyName { get => name; }
    public int MyDamage { get => damage; }
    public Sprite MyIcon { get => icon; }
    public float MySpeed { get => speed;  }
    public GameObject MySpellPrefab { get => spellPrefab; }
    public float MyCastTime { get => castTime; set => castTime = value; }

    public void Use()
    {
        Player.MyInstance.SpellType = MyName;
    }
}
