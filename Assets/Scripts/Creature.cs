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

    public float maxTargetDistance;

    public float collisionCheckDistance = 0.1f;
    // public float obstacleDetectionAngle = 90;
    // [SerializeField]
    // private float obstacleCheckCircleRadius = 3f;
    // [SerializeField]
    // private float obstacleCheckCircleDistance = 3f;

    [SerializeField]
    private LayerMask obstacleLayerMask;
    private RaycastHit2D[] obstacleCollisions;

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

    public Vector2 movementInput;

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
        obstacleCollisions = new RaycastHit2D[10];
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
        currentState?.Update();

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
        statusText.text = currentState.GetName();
    }

    private void FixedUpdate()
    {
        currentState?.FixedUpdate();
        if (!isDead && movementInput != Vector2.zero)
        {
            rb2D.velocity = movementInput * velocity;
        }
    }

    /* *** States *** */
    public void ChangeState()
    {
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
    public void SetMovementInput(Vector2 input)
    {
        if (isDead) return;
        movementInput = input;
        SetFacingDirection();
        isWalking = movementInput.x != 0 || movementInput.y != 0;
    }

    public Vector3 GetMovementDirection(Vector3 target)
    {
        return (target - transform.position).normalized;
    }

    public bool ReachedTarget(Vector3 target, float tolerance = 0.1f)
    {
        return Vector2.Distance(transform.position, target) < tolerance;
    }

    public bool CanMove(Vector2 direction)
    {
        return !WillCollide(direction);
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
        isWalking = false;
    }

    public Vector3 ChooseNewTarget()
    {
        Vector2 target = GeometryUtils.GetRandomPointFromPosition(maxTargetDistance, transform.position, areaMin, areaMax);
        // public LayerMask layerMask; // Optional: restrict which layers to check
        Collider2D[] results = new Collider2D[10]; // Adjust size as needed
        ContactFilter2D filter = new ContactFilter2D();

        if (Physics2D.OverlapPoint(target, filter, results) > 0)
        {
            return ChooseNewTarget();
        }

        return target;
    }

    public bool WillCollide(Vector2 direction)
    {
        bool collisionDetected = touchingCollider.Cast(direction, castFilter, movementHits, collisionCheckDistance) > 0;
        Debug.DrawRay(transform.position, direction * collisionCheckDistance, Color.red);

        Vector3 rotatedVector1 = Quaternion.AngleAxis(15, Vector3.forward)*direction;
        Debug.DrawRay(transform.position, rotatedVector1 * 3, Color.yellow);
        Vector3 rotatedVector2 = Quaternion.AngleAxis(-15, Vector3.forward)*direction;
        Debug.DrawRay(transform.position, rotatedVector2 * 3, Color.yellow);

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
                if (hit.CompareTag(TagStrings.FOOD_TAG))
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

        validObjects.Sort((a, b) =>
            Vector2.Distance(transform.position, a.transform.position)
            .CompareTo(Vector2.Distance(transform.position, b.transform.position)));

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
    private void Die()
    {
        if (isDead) return;

        SetNextState(new DeadState(this));
        isDead = true;
    }

    private void OnDrawGizmosSelected()
    {
        DebugDrawUtils.DrawGizmoCircle(transform.position, inspectionRadius, Color.yellow);
        DebugDrawUtils.DrawGizmoCircle(transform.position, handRange, Color.red);

        Gizmos.color = new Color(0f, 1f, 0f, 0.7f); // Red with custom alpha
        currentState?.DrawGizmos();
    }


}