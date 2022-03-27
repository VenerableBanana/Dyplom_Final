using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MaterialManager
{
    protected Color _color;
	public Color Color => _color;

    protected float _density;
    //public float Density => _density;

    protected bool _flammable;
    //public bool Flammable => _flammable;

    protected float _flowDirection;

    public MaterialManager()
	{
		
	}

    public abstract void CalculatePhysics(Cell currentCell);


    public virtual Color GetColor()
    {
        return _color;
    }

    public virtual void MoveToBottomCell(Cell currentCell)
    {
        currentCell.BottomCell.SetMaterial(currentCell.Material);
        currentCell.SetMaterial(MapGenerator.Palette[0]);
    }

    public virtual void MoveToRightBottomCell(Cell currentCell)
    {
        currentCell.RightBottomCell.SetMaterial(currentCell.Material);
        currentCell.SetMaterial(MapGenerator.Palette[0]);
    }

    public virtual void MoveToLeftBottomCell(Cell currentCell)
    {
        currentCell.LeftBottomCell.SetMaterial(currentCell.Material);
        currentCell.SetMaterial(MapGenerator.Palette[0]);
    }

    public virtual void MoveToRightCell(Cell currentCell)
    {
        currentCell.RightCell.SetMaterial(currentCell.Material);
        currentCell.SetMaterial(MapGenerator.Palette[0]);
    }

    public virtual void MoveToLeftCell(Cell currentCell)
    {
        currentCell.LeftCell.SetMaterial(currentCell.Material);
        currentCell.SetMaterial(MapGenerator.Palette[0]);
    }

    public void MoveToTopCell(Cell currentCell)
    {
        currentCell.TopCell.SetMaterial(currentCell.Material);
        currentCell.SetMaterial(MapGenerator.Palette[0]);
    }

    public void MoveToTopRightCell(Cell currentCell)
    {
        currentCell.TopRightCell.SetMaterial(currentCell.Material);
        currentCell.SetMaterial(MapGenerator.Palette[0]);
    }

    public void MoveToTopLeftCell(Cell currentCell)
    {
        currentCell.TopLeftCell.SetMaterial(currentCell.Material);
        currentCell.SetMaterial(MapGenerator.Palette[0]);
    }

    public virtual void SwapMaterials(Cell cellA, Cell cellB)
    {
        Material temp = cellA.Material;
        cellA.SetMaterial(cellB.NewMaterial);
        cellB.SetMaterial(temp);
    }
}
