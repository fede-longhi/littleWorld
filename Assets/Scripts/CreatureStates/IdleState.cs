using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IdleState : CreatureState
{
    private float waitTime;

    public IdleState(Creature creature) : base(creature) { }

    public override void Enter()
    {
        creature.StopMoving();
        waitTime = Random.Range(0.5f, creature.maxTimeBetweenMoves);
    }

    public override void Update()
    {
        waitTime -= Time.deltaTime;

        if (waitTime <= 0f)
        {
            creature.ChangeState();
        }
    }

    public override void FixedUpdate() { }

    public override void Exit() { }

    public override string GetName() => "idle";
}