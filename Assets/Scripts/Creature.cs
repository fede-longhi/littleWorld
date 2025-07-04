using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class Creature : MonoBehaviour
{
    public TMP_Text statusText;
    private Rigidbody2D rb2D;
    private Animator animator;
    CapsuleCollider2D touchingCollider;
    public ContactFilter2D castFilter;
    RaycastHit2D[] movementHits = new RaycastHit2D[5];

    public float velocity = 1f;
    public float maxTimeBetweenMoves = 1f;
    public Vector2 areaMin;
    public Vector2 areaMax;

    public float collisionCheckDistance = 0.1f;

    public Vector3 TargetPosition;
    public bool movingTowardsTarget = false;

    private CreatureState currentState;

    [Header("Creature Needs")]
    public float life = 100f;
    [Header("Hunger")]
    public float hunger = 0f;
    public float hungerRate = 1f; // per second
    public float hungerThreshold = 50f; // gets "hungry" at this value
    public float maxHunger = 100f;
    public float hungerDamageRate = 1f;

    [Header("Inspection")]
    public float inspectionRadius = 2f;
    public float handRange = 0.2f;

    public bool IsHungry => hunger >= hungerThreshold;

    public List<Func<CreatureState>> possibleCreatureStates;


    private Vector2 movementInput;

    private bool _isWalking = false;
    public bool isWalking
    {
        get
        {
            return _isWalking;
        }
        private set
        {
            _isWalking = value;
            animator.SetBool(AnimationStrings.walking, value);
        }
    }

    private bool _isEating = false;
    public bool isEating
    {
        get
        {
            return _isEating;
        }
        private set
        {
            _isEating = value;
            animator.SetBool(AnimationStrings.eating, value);
        }
    }

    private bool _isDead = false;
    public bool isDead
    {
        get
        {
            return _isDead;
        }
        private set
        {
            _isDead = value;
            animator.SetBool(AnimationStrings.isDead, value);
        }
    }

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingCollider = GetComponent<CapsuleCollider2D>();
        possibleCreatureStates = new List<Func<CreatureState>>
        {
            () => new IdleState(this),
            () => new WalkingState(this),
            () => new SeekingFoodState(this)
        };
        ChangeState();
    }

    void Update()
    {
        hunger += hungerRate * Time.deltaTime;
        hunger = Mathf.Min(hunger, maxHunger);

        if (hunger > hungerThreshold)
        {
            life -= hungerDamageRate * Time.deltaTime;
            life = Mathf.Max(life, 0f); // Prevent negative life
        }

        if (life <= 0f)
        {
            Die();
        }

        currentState?.Update();

        statusText.text = currentState.GetName();
    }

    public void SetMoventInput(Vector2 input)
    {
        movementInput = input;
        SetFacingDirection();
        isWalking = movementInput.x != 0 || movementInput.y != 0;
    }

    private void FixedUpdate()
    {
        if (!isDead && movementInput != Vector2.zero)
        {
            rb2D.velocity = movementInput * velocity;
        }

        currentState?.FixedUpdate();
    }

    /* *** States *** */
    public void ChangeState()
    {
        //TODO: move this on the determine next state func
        if (IsHungry)
        {
            // Vector3 foodPosition = FindNearbyFood(); // implement this later
            SetNextState(new SeekingFoodState(this));
            return;
        }

        CreatureState nextState = DetermineNextState();

        SetNextState(nextState);
    }

    public void SetNextState(CreatureState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    private CreatureState DetermineNextState()
    {
        if (possibleCreatureStates == null || possibleCreatureStates.Count == 0)
            return new IdleState(this);

        int index = Random.Range(0, possibleCreatureStates.Count);
        return possibleCreatureStates[index]();
    }

    /* *** Movement *** */

    public Vector2 GetDirectionToTarget()
    {
        return (TargetPosition - transform.position).normalized;
    }

    public bool ReachedTarget(float tolerance = 0.1f)
    {
        return movingTowardsTarget && Vector2.Distance(transform.position, TargetPosition) < tolerance;
    }

    public bool CanMove(Vector2 direction)
    {
        return !WillCollide(direction);
    }

    public void Move()
    {
        SetMovementDirection(GetDirectionToTarget());
    }

    public void SetMovementDirection(Vector2 direction)
    {
        if (isDead) return;

        isWalking = direction != Vector2.zero;
        movementInput = direction;
        SetFacingDirection();
    } 

    private void SetFacingDirection()
    {
        if (movementInput.x < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            statusText.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (movementInput.x > 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            statusText.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void StopMoving()
    {
        rb2D.velocity = Vector2.zero;
        movementInput = Vector2.zero;
        movingTowardsTarget = false;
        isWalking = false;
    }

    public void ChooseNewTarget()
    {
        float destinationX = Random.Range(areaMin.x, areaMax.x);
        float destinationY = Random.Range(areaMin.y, areaMax.y);

        TargetPosition = new Vector3(destinationX, destinationY, 0);

        movingTowardsTarget = true;
    }

    public bool WillCollide(Vector2 direction)
    {
        bool collisionDetected = touchingCollider.Cast(direction, castFilter, movementHits, collisionCheckDistance) > 0;
        Debug.DrawRay(transform.position, direction * collisionCheckDistance, Color.red);

        return collisionDetected;
    }

    /* *** Food *** */
    public void Eat()
    {
        isEating = true;
    }

    public void StopEating()
    {
        isEating = false;
    }

    public bool CanEat()
    {
        if (HasFood())
        {
            return true;
        }
        else
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, handRange);
            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject.tag == TagStrings.FOOD_TAG)
                {
                    return true;
                }
            }
            return false;
        }
    }

    private bool HasFood()
    {
        return false;
    }

    /* *** Inspection *** */
    public Dictionary<string, List<GameObject>> Inspect()
    {
        Dictionary<string, List<GameObject>> groupedObjects = new();

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, inspectionRadius);

        List<GameObject> validObjects = new();

        foreach (Collider2D hit in hits)
        {
            GameObject obj = hit.gameObject;
            if (obj != this.gameObject) // exclude self
            {
                validObjects.Add(obj);
            }
        }

        // Sort by distance (closest first)
        validObjects.Sort((a, b) =>
            Vector2.Distance(transform.position, a.transform.position)
            .CompareTo(Vector2.Distance(transform.position, b.transform.position)));

        // Group by tag
        foreach (GameObject obj in validObjects)
        {
            string tag = obj.tag;

            if (!groupedObjects.ContainsKey(tag))
            {
                groupedObjects[tag] = new List<GameObject>();
            }

            groupedObjects[tag].Add(obj);
        }

        return groupedObjects;
    }

    private void OnDrawGizmosSelected()
    {
        DebugDrawUtils.DrawGizmoCircle(transform.position, inspectionRadius, Color.yellow);
        DebugDrawUtils.DrawGizmoCircle(transform.position, handRange, Color.red);
    }

    private void Die()
    {
        SetNextState(new DeadState(this));
        isDead = true;
    }

}

public enum WalkResult
{
    None,
    Collided,
    ReachedTarget
}