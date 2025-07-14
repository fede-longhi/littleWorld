using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class MovingToTargetState : CreatureState
{
    protected Vector3 targetPosition;
    private Vector3 lastDirection;
    private Direction lastChosenDirection;
    private bool isAvoiding = false;

    public MovingToTargetState(Creature creature) : base(creature) { }

    public override void Enter()
    {
        targetPosition = GetInitialTarget();
    }

    public override void Update()
    {
        if (ShouldInterrupt())
            return;

        Vector2 direction = creature.GetMovementDirection(targetPosition);

        if (creature.CanMove(direction))
        {
            isAvoiding = false;
            creature.SetMovementInput(direction);
        }
        else
        {
            Vector2 alternative = FindAlternativeDirection(direction);
            lastDirection = alternative;
            creature.SetMovementInput(alternative);
            isAvoiding = true;
        }
    }

    public override void FixedUpdate()
    {
        if (creature.ReachedTarget(targetPosition))
        {
            OnReachedTarget();
        }
    }

    public override void Exit()
    {
        creature.StopMoving();
    }

    public override void DrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 1f);
        Gizmos.DrawSphere(targetPosition, 0.1f);
    }

    protected Vector2 FindAlternativeDirection(Vector2 originalDirection)
    {
        float maxAngle = 360f;
        float angleStep = 15f;
        int maxIterations = (int)Mathf.Floor(maxAngle / angleStep);
        for (int i = 0; i <= maxIterations; i++)
        {
            float angle = angleStep * i;
            if ((isAvoiding && lastChosenDirection == Direction.RIGHT) || !isAvoiding)
            {
                Vector2 rotatedRight = Quaternion.Euler(0, 0, angle) * originalDirection;
                if (creature.CanMove(rotatedRight) && !GeometryUtils.IsOppositeDirection(rotatedRight, lastDirection, angleStep))
                {
                    lastChosenDirection = Direction.RIGHT;
                    return rotatedRight;
                }
            }

            if ((isAvoiding && lastChosenDirection == Direction.LEFT) || !isAvoiding)
            {
                Vector2 rotatedLeft = Quaternion.Euler(0, 0, -angle) * originalDirection;
                if (creature.CanMove(rotatedLeft) && !GeometryUtils.IsOppositeDirection(rotatedLeft, lastDirection, angleStep))
                {
                    lastChosenDirection = Direction.LEFT;
                    return rotatedLeft;
                }
            }
        }
        return Vector2.zero;
    }

    protected abstract Vector3 GetInitialTarget();
    protected abstract void OnReachedTarget();
    protected virtual bool ShouldInterrupt() => false;

    private enum Direction
    {
        LEFT,
        RIGHT
    }
}
