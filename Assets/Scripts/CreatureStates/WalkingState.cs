using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : CreatureState
{
    public WalkingState(Creature creature) : base(creature) { }

    public override void Enter()
    {
        creature.ChooseNewTarget();
    }

    public override void Update()
    {
        Vector2 direction = creature.GetDirectionToTarget();
        creature.SetMoventInput(direction);
    }

    public override void FixedUpdate()
    {
        Vector2 direction = creature.GetDirectionToTarget();
        if (creature.ReachedTarget() || !creature.CanMove(direction))
        {
            creature.ChangeState();
            return;
        }
    }

    public override void Exit()
    {
        creature.StopMoving();
    }

    public override string GetName() => "walking";
}