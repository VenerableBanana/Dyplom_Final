using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Profiling;

public class Cell
{

    public int _x;
    public int _y;
    public Material Material;
    public Material NewMaterial;
    Cell[,] _map;
    private bool _isUpdated = false;
    public Chunk MyChunk;

    public bool IsUpdated
    {
        get { return _isUpdated; }
        set
        {
            _isUpdated = value;
        }
    }

    public bool HasChanged { get; set; } = false;

    public Cell BottomCell;
    public Cell RightCell;
    public Cell LeftCell;
    public Cell TopCell;
    public Cell TopRightCell;
    public Cell TopLeftCell;
    public Cell RightBottomCell;
    public Cell LeftBottomCell;

    public static Action OnCellUpdated;
    public Cell(int x, int y, Cell[,] map, Material material, Chunk myChunk)
    {
        _x = x;
        _y = y;
        MyChunk = myChunk;
        _map = map;
        IsUpdated = false;
        HasChanged = true;
        Material = material;
        NewMaterial = material;
    }

    public void GetAllNeighbours()
    {
        BottomCell = GetNeighbour(0, -1);
        RightCell = GetNeighbour(1, 0);
        LeftCell = GetNeighbour(-1, 0);
        TopCell = GetNeighbour(0, 1);
        TopRightCell = GetNeighbour(1, 1);
        TopLeftCell = GetNeighbour(-1, 1);
        RightBottomCell = GetNeighbour(1, -1);
        LeftBottomCell = GetNeighbour(-1, -1);
    }

    public Cell GetNeighbour(int x, int y)
    {
        if (_y + y > 0 && _x + x > 0 && _y + y < _map.GetLength(1) && _x + x < _map.GetLength(0))
        {
            return _map[_x + x, _y + y];
        }

        return null;
    }

    public void UpdateMaterial()
    {
        Material = NewMaterial;
        IsUpdated = false;
    }

    public void SetMaterial(Material material)
    {
        NewMaterial = new Material(material.Type, material.Color, material.Density, material.Counter, material.Flammable, material.FlowDirection);
        IsUpdated = true;
        HasChanged = true;
        MyChunk.SetIsUpdated(true);
    }
}
