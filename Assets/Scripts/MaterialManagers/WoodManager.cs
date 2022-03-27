using UnityEngine;

public class WoodManager : MaterialManager
{

    public WoodManager() : base()
    {
        _color = new Color(0.39f, 0.24f, 0.05f);
        _flammable = true;
        _density = 1800f;
    }

    public override void CalculatePhysics(Cell currentCell)
    {
        currentCell.IsUpdated = true;
        currentCell.HasChanged = false;
    }
}
