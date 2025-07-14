using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingFoodState : MovingToTargetState
{
    public SeekingFoodState(Creature creature) : base(creature) { }

    protected override Vector3 GetInitialTarget()
    {
        return creature.ChooseNewTarget();
    }

    protected override bool ShouldInterrupt()
    {
        if (creature.CanEat())
        {
            creature.SetNextState(new EatingState(creature));
            return true;
        }

        Dictionary<string, List<GameObject>> detected = creature.Inspect();
        if (detected.ContainsKey(TagStrings.FOOD_TAG))
        {
            targetPosition = detected[TagStrings.FOOD_TAG][0].transform.position;
        }

        return false;
    }

    protected override void OnReachedTarget()
    {
        // Keep moving randomly if no food found
        targetPosition = creature.ChooseNewTarget();
    }

    public override string GetName() => "seeking food";
}