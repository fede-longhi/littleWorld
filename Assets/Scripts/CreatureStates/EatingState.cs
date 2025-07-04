
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingState : CreatureState
{
    private float eatTime = 2f;
    private float timer;

    public EatingState(Creature creature) : base(creature) { }

    public override void Enter()
    {
        creature.StopMoving();
        creature.Eat();
        timer = eatTime;
    }

    public override void FixedUpdate() { }
    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            creature.hunger = 0f;
            creature.ChangeState();
        }
    }

    public override void Exit()
    {
        creature.StopEating();
    }

    public override string GetName() => "eating";
}