using System;
using UnityEngine;

public class CreateCreatureAction : GameAction
{
    private GameObject prefab;
    private Camera camera;
    private Action<GameObject> registerCallback;

    public CreateCreatureAction(GameObject prefab, Camera camera, Action<GameObject> registerCallback)
    {
        this.prefab = prefab;
        this.camera = camera;
        this.registerCallback = registerCallback;
        this.type = GameActionType.CREATE_CREATURE;
    }

    public override void Execute(Vector2 mousePosition)
    {
        float zDistance = Mathf.Abs(camera.transform.position.z);
        Vector3 worldPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, zDistance));
        worldPosition.z = 0;

        GameObject creature = GameObject.Instantiate(prefab, worldPosition, Quaternion.identity);
        registerCallback?.Invoke(creature);
    }
}