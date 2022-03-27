using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireManager : MaterialManager
{

    private readonly float _framesToDisappear;
    private float _counter;

    public FireManager()
    {
        var c = GenerateColor();
        _framesToDisappear = 2000f;
        _counter = 0f;
        _flammable = false;
    }

    public override void CalculatePhysics(Cell currentCell)
    {
        var color = GenerateColor();
        currentCell.Material.Color = new Color(color.Item1, color.Item2, 0f);
        currentCell.Material.Counter++;

        if (currentCell.TopCell != null &&
            currentCell.TopCell.NewMaterial.Type == 0)
        {
            var rng = Random.Range(0f, 100f);

            if (rng <= 1f)
            {
                currentCell.TopCell.SetMaterial(MapGenerator.Palette[4]);
            }        
        }

        if (currentCell.Material.Counter <= _framesToDisappear)
        {
            List<Cell> directions = new List<Cell>();

            if (currentCell.BottomCell != null &&
                currentCell.BottomCell.NewMaterial.Flammable)
                directions.Add(currentCell.BottomCell);

            if (currentCell.TopCell != null &&
                currentCell.TopCell.NewMaterial.Flammable)
                directions.Add(currentCell.TopCell);

            if (currentCell.RightCell != null &&
                currentCell.RightCell.NewMaterial.Flammable)
                directions.Add(currentCell.RightCell);

            if (currentCell.LeftCell != null &&
                currentCell.LeftCell.NewMaterial.Flammable)
                directions.Add(currentCell.LeftCell);

            foreach (var element in directions)
            {
                var rng = Random.Range(0f, 500f);
                if (rng <= 2.5f)
                {
                    element.SetMaterial(MapGenerator.Palette[9]);
                }
            }
        }
        else
        {
            currentCell.SetMaterial(MapGenerator.Palette[0]);
            return;
        }
        currentCell.SetMaterial(currentCell.Material);
    }

    private (float, float) GenerateColor()
    {
        var rng = Random.Range(0.65f, 0.99f);
        var r = rng;
        rng = Random.Range(0f, 0.18f);
        var g = rng;

        return (r, g);
    }
}
