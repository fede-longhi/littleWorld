using System;
using UnityEngine;

public class SelectEntityAction : GameAction
{
    private static readonly float selectionRadius = 0.5f;
    private Action<GameObject> registerCallback;
    private GameObject selectedObject;

    public SelectEntityAction(Action<GameObject> registerCallback)
    {
        this.registerCallback = registerCallback;
    }

    public override void Execute(Vector2 position)
    {
        Debug.Log($"Selecting entity at position: {position}");
        Vector3 worldPosition = GeometryUtils.ScreenToWorldPosition(position, Camera.main);
        Collider2D hit = Physics2D.OverlapCircle(worldPosition, selectionRadius);
        if (hit != null)
        {
            Debug.Log("Hit detected: " + hit.gameObject.name);
            selectedObject = hit.gameObject;
        }
        else
        {
            selectedObject = null;
        }
        registerCallback?.Invoke(selectedObject);
    }
}