using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public struct Material
{
    public uint Type;
    public float FlowDirection;
    public float Density;
    public int Counter;
    public bool Flammable;
    public Color Color;

    public Material(uint type, Color defaultColor, float density = 0, int counter = -1, bool flammable = false, float flowDirection = 0f)
    {
        Type = type;
        Density = density;
        Flammable = flammable;
        FlowDirection = flowDirection;
        Counter = counter;
        Color = defaultColor;
    }
}

