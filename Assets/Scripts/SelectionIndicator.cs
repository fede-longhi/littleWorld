using UnityEngine;
using System;

public class SelectionIndicator : MonoBehaviour
{
    private Entity selectedEntity;

    public void Update()
    {
        if (selectedEntity != null)
        {
            this.transform.position = selectedEntity.transform.position + new Vector3(0, selectedEntity.selectionTagPositionOffset, 0);
        }
    }

    public void SetSelectedEntity(Entity selectedEntity)
    {
        this.selectedEntity = selectedEntity;
    }
}