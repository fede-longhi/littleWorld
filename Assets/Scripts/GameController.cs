using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class GameController : MonoBehaviour
{
    public GameObject creaturePrefab;
    public GameObject activeEntityPrefab;
    [SerializeField]
    private CinemachineVirtualCamera activeCamera;
    private List<GameObject> creatures = new List<GameObject>();
    private GameObject cameraTarget;
    private GameObject activeCreature;
    private int creatureIndex;
    private GameAction activeGameAction;
    public GameStats stats;
    private bool freeCamera = true;

    private void Awake()
    {
        Camera.main.gameObject.TryGetComponent<CinemachineBrain>(out var brain);
        brain ??= Camera.main.gameObject.AddComponent<CinemachineBrain>();
        brain.m_DefaultBlend.m_Time = 1;

        creatureIndex = 0;
        GameObject[] initialCreatureObjects = GameObject.FindGameObjectsWithTag(TagStrings.CREATURE_TAG);
        foreach (GameObject creatureObject in initialCreatureObjects)
        {
            creatureObject.TryGetComponent<Creature>(out Creature creature);
            RegisterCreature(creatureObject);
        }

        GameObject[] cameraTargets = GameObject.FindGameObjectsWithTag(TagStrings.CAMERA_FOLLOW_TAG);
        if (cameraTargets.Length > 0)
        {
            cameraTarget = cameraTargets[0];
            activeCamera.Follow = cameraTarget.transform;
        }

        activeGameAction = new SelectEntityAction(SelectEntity);
    }

    public void SelectEntity(GameObject entityObject)
    {
        if (entityObject != null)
        {
            if (activeCreature != null && activeCreature != entityObject)
            {
                freeCamera = false;
                activeCreature.TryGetComponent<Entity>(out Entity activeEntity);
                activeEntity?.Deselect();
                GameEventBus.Raise(new GameEvent { type = GameEventType.DESELECTED_ENTITY, data = activeEntity });
            }

            activeCreature = entityObject;
            entityObject.TryGetComponent<Entity>(out Entity entity);
            if (entity != null)
            {
                activeCamera.Follow = activeCreature.transform;
                Debug.Log($"Selected Entity: {entity.entityName}");
                entity.Select();
                GameEventBus.Raise(new GameEvent { type = GameEventType.SELECTED_ENTITY, data = entity });
            }
            else
            {
                Debug.Log("Selected an object that is not an Entity.");
            }
        }
        else if (activeCreature != null)
        {
            activeCreature.TryGetComponent<Entity>(out Entity activeEntity);
            cameraTarget.transform.position = activeCreature.transform.position;
            activeCamera.Follow = cameraTarget.transform;
            freeCamera = true;
            activeEntity?.Deselect();
            GameEventBus.Raise(new GameEvent { type = GameEventType.DESELECTED_ENTITY, data = activeEntity });
        }
        else
        {
            activeCamera.Follow = cameraTarget.transform;
            freeCamera = true;
        }
    }

    public void OnChangeTarget(InputAction.CallbackContext context)
    {
        if (context.started && !freeCamera)
        {
            Debug.Log("Change Target");
            creatureIndex += 1;
            creatureIndex %= creatures.Count;
            activeCreature = creatures[creatureIndex];
            activeCamera.Follow = activeCreature.transform;
        }
    }

    public void SetGameAction(GameAction action)
    {
        activeGameAction = action;
    }

    public void RegisterCreature(GameObject creatureObject)
    {
        if (!creatures.Contains(creatureObject))
        {
            creatures.Add(creatureObject);
            stats.IncreasePopulation(); //TODO: see if we can improve this
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed && activeGameAction != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            activeGameAction.Execute(mousePosition);
        }
    }

    public void OnSelectCreateCreature()
    {
        Debug.Log("Create creature");
        SetGameAction(new CreateCreatureAction(creaturePrefab, Camera.main, RegisterCreature));
    }

    public void OnClearGameAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (activeGameAction != null)
            {
                activeGameAction = new SelectEntityAction(SelectEntity);
            }

            if (activeCreature != null)
            {
                activeCreature.TryGetComponent<Entity>(out Entity activeEntity);
                cameraTarget.transform.position = activeCreature.transform.position;
                activeCamera.Follow = cameraTarget.transform;
                freeCamera = true;
                activeEntity?.Deselect();
                GameEventBus.Raise(new GameEvent { type = GameEventType.DESELECTED_ENTITY, data = activeEntity });
            }
        }
    }

    public void OnSetNormalTime()
    {
        Time.timeScale = 1f;
    }

    public void OnSetSlowestTimeScale()
    {
        Time.timeScale = 0.1f;
    }

    public void OnSetFastestTimeScale()
    {
        Time.timeScale = 50f;
    }

    public void OnIncreaseTimeSpeed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IncreaseTimeSpeed();
        }
    }

    public void OnDecreaseTimeSpeed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DecreaseTimeSpeed();
        }
    }
    public void IncreaseTimeSpeed()
    {
        if (Time.timeScale < 50)
        {
            Time.timeScale *= 2;
        }
    }

    public void DecreaseTimeSpeed()
    {
        if (Time.timeScale > 0.1)
        {
            Time.timeScale /= 2f;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (freeCamera)
        {
            Vector2 movementInput = context.ReadValue<Vector2>();
            if (cameraTarget != null)
            {
                cameraTarget.TryGetComponent<Movable>(out var movable);
                movable?.Move(new Vector3(movementInput.x, movementInput.y, 0).normalized);
            }
            
            // if (movementInput.x > -0.5f && movementInput.x < 0.5f)
            // {
            //     movementInput.x = 0f;
            // }

            // if (movementInput.y > -0.5f && movementInput.y < 0.5f)
            // {
            //     movementInput.y = 0f;
            // }
        }
    }
}