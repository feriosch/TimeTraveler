using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    private static SpellBook instance;

    public static SpellBook MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpellBook>();
            }

            return instance;
        }
    }
    [SerializeField]
    private Spell[] spells;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Spell CastSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.MyName == spellName);
        return spell;
    }

    public Spell GetSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.MyName == spellName);
        return spell;
    }
}
