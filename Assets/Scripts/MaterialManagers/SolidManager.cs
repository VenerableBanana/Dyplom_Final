using UnityEngine;

public class SolidManager : MaterialManager
{
	public SolidManager() : base()
	{
		_color = Color.gray;
        _density = 10000f;
        _flammable = false;
    }

	public override void CalculatePhysics(Cell currentCell)
	{
		currentCell.HasChanged = false;
		currentCell.IsUpdated = true;
	}
}
