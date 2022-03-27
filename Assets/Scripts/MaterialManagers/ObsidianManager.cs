using UnityEngine;

public class ObsidianManager : MaterialManager
{

    public ObsidianManager() : base()
    {
        _color = new Color(0f, 0f, 0.19f);
        _flammable = false;
        _density = 1800f;
    }

    public override void CalculatePhysics(Cell currentCell)
    {

    }
}
