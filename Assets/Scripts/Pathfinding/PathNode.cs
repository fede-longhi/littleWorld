using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public Vector2 Position;
    public List<PathNode> Neighbors = new();
    public float GCost;
    public float HCost;
    public float FCost => GCost + HCost;
    public PathNode CameFrom;
}