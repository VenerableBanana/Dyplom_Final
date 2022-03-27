using UnityEngine;

public class EmptyManager : MaterialManager
{
	public EmptyManager() : base()
	{
		_color = Color.black;
        _density = 0f;
        _flammable = false;
    }

	public override void CalculatePhysics(Cell currentCell)
	{
		currentCell.IsUpdated = true;
		currentCell.HasChanged = false;
	}

}
