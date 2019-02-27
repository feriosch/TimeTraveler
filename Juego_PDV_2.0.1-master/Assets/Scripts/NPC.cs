using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Character
{
    public virtual void DeSelect()
    {
    }

    public virtual Transform Select() //virtual es para sobreescribir despues
    {
        return hitBox;
    }
}
