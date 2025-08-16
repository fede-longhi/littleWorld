using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public string entityName;
    public float selectionTagPositionOffset = 0.5f;

    public virtual void Select() { }

    public virtual void Deselect() {}
}