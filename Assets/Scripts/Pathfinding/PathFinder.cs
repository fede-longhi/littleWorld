using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pathfinder {
    public static List<Vector2> FindPath(PathNode start, PathNode goal) {
        var openSet = new List<PathNode> { start };
        var closedSet = new HashSet<PathNode>();

        start.GCost = 0;
        start.HCost = Vector2.Distance(start.Position, goal.Position);

        while (openSet.Count > 0) {
            openSet.Sort((a, b) => a.FCost.CompareTo(b.FCost));
            var current = openSet[0];

            if (current == goal) {
                return ReconstructPath(goal);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in current.Neighbors) {
                if (closedSet.Contains(neighbor)) continue;

                float tentativeG = current.GCost + Vector2.Distance(current.Position, neighbor.Position);

                if (!openSet.Contains(neighbor)) {
                    openSet.Add(neighbor);
                } else if (tentativeG >= neighbor.GCost) {
                    continue;
                }

                neighbor.CameFrom = current;
                neighbor.GCost = tentativeG;
                neighbor.HCost = Vector2.Distance(neighbor.Position, goal.Position);
            }
        }

        return null;
    }

    private static List<Vector2> ReconstructPath(PathNode end) {
        var path = new List<Vector2>();
        PathNode current = end;

        while (current != null) {
            path.Insert(0, current.Position);
            current = current.CameFrom;
        }

        return path;
    }
}
