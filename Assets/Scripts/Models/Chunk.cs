using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public bool HasBeenUpdated;
    public bool IsUpdated;
    public int MaxX;
    public int MaxY;
    public int MinX;
    public int MinY;
    public int _x;
    public int _y;
    Chunk[,] _chunks;

    public Chunk(int maxX, int maxY, int minX, int minY, int x, int y, Chunk[,] chunks)
    {
        MinX = minX;
        MinY = minY;
        MaxX = maxX;
        MaxY = maxY;
        IsUpdated = true;
        HasBeenUpdated = true;
        _chunks = chunks;
        _x = x;
        _y = y;
    }

    public void SetIsUpdated(bool isUpdated)
    {
        IsUpdated = isUpdated;
        if (isUpdated)
        {
            int row_limit = _chunks.GetLength(0);
            if (row_limit > 0)
            {
                int column_limit = _chunks.GetLength(1);
                for (int x = Mathf.Max(0, _x - 1); x <= Mathf.Min(_x + 1, row_limit - 1); x++)
                {
                    for (int y = Mathf.Max(0, _y - 1); y <= Mathf.Min(_y + 1, column_limit - 1); y++)
                    {
                        if (x != _x || y != _y)
                        {
                            _chunks[x, y].IsUpdated = true;
                        }
                    }
                }
            }
        }
    }
}
