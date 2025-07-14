using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphBuilder : MonoBehaviour
{
    public LayerMask obstacleMask;
    public float nodeRadius = 1f;
    public float connectionRadius = 5f;
    public float sampleRadius = 10f;

    public List<PathNode> GenerateGraph(Vector2 origin, Vector2 target)
    {
        List<PathNode> nodes = new();

        

        // Agregamos el nodo de inicio y objetivo
        PathNode start = new PathNode { Position = origin };
        PathNode goal = new PathNode { Position = target };
        nodes.Add(start);
        nodes.Add(goal);

        // Obtenemos colliders cercanos
        Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, sampleRadius, obstacleMask);

        foreach (var col in colliders)
        {
            Vector2 point = col.ClosestPoint(origin);

            // Podrías muestrear más puntos alrededor si querés más precisión
            PathNode node = new PathNode { Position = point };
            nodes.Add(node);
        }

        // Conectamos nodos con línea de visión libre
        foreach (var a in nodes)
        {
            foreach (var b in nodes)
            {
                if (a == b) continue;

                if (!Physics2D.Linecast(a.Position, b.Position, obstacleMask))
                {
                    if (!a.Neighbors.Contains(b)) a.Neighbors.Add(b);
                }
            }
        }

        return nodes;
    }
}
