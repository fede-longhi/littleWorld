using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatureState
{
    protected Creature creature;

    public CreatureState(Creature creature)
    {
        this.creature = creature;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();
    public abstract string GetName();
}