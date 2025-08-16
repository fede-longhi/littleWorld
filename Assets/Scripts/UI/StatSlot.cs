using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;


public class StatSlot : MonoBehaviour
{
    public TextMeshProUGUI labelMesh;
    public TextMeshProUGUI valueMesh;
    public Func<string> updateFunction;


    public void SetLabel(string label)
    {
        labelMesh.text = label;
    }
    public void SetValue(string value)
    {
        valueMesh.text = value;
    }
}