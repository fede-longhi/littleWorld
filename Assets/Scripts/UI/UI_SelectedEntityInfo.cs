using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class UI_SelectedEntityInfo : MonoBehaviour
{
    public TextMeshProUGUI nameMesh;
    public GameController gameController;

    public void Awake()
    {
        GameEventBus.OnSelectedEntity += HandleSelectedEntity;
    }

    public void HandleSelectedEntity(GameEvent evt)
    {
        if (evt.data is Entity entity)
        {
            nameMesh.text = entity.entityName;
        }
    }
}