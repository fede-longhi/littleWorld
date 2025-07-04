using UnityEngine;

public static class DebugDrawUtils
{
    public static void DrawGizmoCircle(Vector3 center, float radius, Color color, int segments = 32)
    {
        Gizmos.color = color;

        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0), Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float rad = Mathf.Deg2Rad * (i * angleStep);
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
