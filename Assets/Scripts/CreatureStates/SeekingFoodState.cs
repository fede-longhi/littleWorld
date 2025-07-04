using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingFoodState : CreatureState
{
    public SeekingFoodState(Creature creature) : base(creature) { }

    public override void Enter()
    {
        creature.ChooseNewTarget();
        creature.movingTowardsTarget = true;
    }

    public override void Update()
    {
        if (creature.CanEat())
        {
            creature.SetNextState(new EatingState(creature));
            return;
        }

        Dictionary<string, List<GameObject>> objectsDetected = creature.Inspect();

        if (objectsDetected.ContainsKey(TagStrings.FOOD_TAG))
        {
            creature.TargetPosition = objectsDetected[TagStrings.FOOD_TAG][0].transform.position;
        }

        Vector2 direction = creature.GetDirectionToTarget();
        creature.SetMovementDirection(direction);
    }

    public override void FixedUpdate() {
        if (creature.ReachedTarget())
        {
            creature.ChooseNewTarget();
            creature.movingTowardsTarget = true;
        }
    }

    public override void Exit()
    {
        creature.StopMoving();
    }

    public override string GetName() => "seeking food";
}