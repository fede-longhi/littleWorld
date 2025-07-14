using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : MovingToTargetState
{
    public WalkingState(Creature creature) : base(creature) { }

    protected override Vector3 GetInitialTarget()
    {
        return creature.ChooseNewTarget();
    }

    protected override void OnReachedTarget()
    {
        creature.ChangeState();
    }

    public override string GetName() => "walking";
}