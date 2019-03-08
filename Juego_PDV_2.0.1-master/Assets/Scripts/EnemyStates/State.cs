using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public abstract void Update();

    public abstract void Exit();
}
