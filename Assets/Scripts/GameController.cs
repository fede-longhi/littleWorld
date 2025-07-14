using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class GameController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera activeCamera;
    private GameObject[] creatures;
    private GameObject activeCreature;
    private int creatureIndex;

    private void Awake()
    {
        Camera.main.gameObject.TryGetComponent<CinemachineBrain>(out var brain);
        if (brain == null)
        {
            brain = Camera.main.gameObject.AddComponent<CinemachineBrain>();
        }
        brain.m_DefaultBlend.m_Time = 1;

        creatureIndex = 0;
        creatures = GameObject.FindGameObjectsWithTag("people");
        if (creatures.Length > 0)
        {
            activeCreature = creatures[creatureIndex];
            activeCamera.Follow = activeCreature.transform;
        }
    }

    public void OnChangeTarget(InputAction.CallbackContext context)
    {
        creatureIndex += 1;
        creatureIndex %= creatures.Length;
        activeCreature = creatures[creatureIndex];
        activeCamera.Follow = activeCreature.transform;
        Debug.Log("Change target");
    }
}