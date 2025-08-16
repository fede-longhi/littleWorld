using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class GeometryUtils
{
    public static bool IsOppositeDirection(Vector2 dirA, Vector2 dirB, float toleranceDegrees = 5f)
    {
        dirA.Normalize();
        dirB.Normalize();
        float angle = Vector2.Angle(dirA, dirB);

        return Mathf.Abs(angle - 180f) <= toleranceDegrees;
    }

    public static Vector2 GetRandomPointFromPosition(float maxDistance, Vector2 position, Vector2 areaMin, Vector2 areaMax)
    {
        float angleDegrees = Random.Range(0, 360);
        float distance = Random.Range(0, maxDistance);

        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        float x = position.x + Mathf.Cos(angleRadians) * distance;
        float y = position.y + Mathf.Sin(angleRadians) * distance;

        float destinationX = Mathf.Max(Mathf.Min(x, areaMax.x), areaMin.x);
        float destinationY = Mathf.Max(Mathf.Min(y, areaMax.y), areaMin.y);

        return new Vector2(destinationX, destinationY);
    }

    public static Vector3 ScreenToWorldPosition(Vector2 position, Camera camera)
    {
        float zDistance = Mathf.Abs(camera.transform.position.z);
        Vector3 worldPosition = camera.ScreenToWorldPoint(new Vector3(position.x, position.y, zDistance));
        worldPosition.z = 0;
        return worldPosition;
    }
}