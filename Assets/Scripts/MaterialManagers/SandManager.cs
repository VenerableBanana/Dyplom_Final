using UnityEngine;

public class SandManager : MaterialManager
{

    public SandManager() : base()
    {
        _color = Color.yellow;
        _density = 1520f;
        _flammable = false;
    }

    public override void CalculatePhysics(Cell currentCell)
    {
        if (currentCell.BottomCell != null)
        {
            if (currentCell.BottomCell.NewMaterial.Type == 0)
            {
                MoveToBottomCell(currentCell);
                return;
            }
            else
            {
                if (currentCell.BottomCell.NewMaterial.Type == 3)
                {

                    var rng = Random.Range(0f, 2f);

                    if (rng >= 1f)
                    {
                        if (currentCell.RightBottomCell != null &&
                            currentCell.RightBottomCell.NewMaterial.Type != 1)
                        {
                            if (currentCell.RightBottomCell.NewMaterial.Type == 0)
                            {
                                MoveToRightBottomCell(currentCell);
                                return;
                            }
                            else if (currentCell.RightBottomCell.NewMaterial.Type != 0)
                            {
                                if (currentCell.RightBottomCell.Material.Density < _density)
                                {
                                    SwapMaterials(currentCell, currentCell.RightBottomCell);
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (currentCell.LeftBottomCell != null &&
                            currentCell.LeftBottomCell.NewMaterial.Type != 1)
                        {
                            if (currentCell.LeftBottomCell.NewMaterial.Type == 0)
                            {
                                MoveToLeftBottomCell(currentCell);
                                return;
                            }
                            else if (currentCell.LeftBottomCell.NewMaterial.Type != 0)
                            {
                                if (currentCell.LeftBottomCell.Material.Density < _density)
                                {
                                    SwapMaterials(currentCell, currentCell.LeftBottomCell);
                                    return;
                                }
                            }
                        }
                    }

                }
                else if (currentCell.BottomCell.NewMaterial.Type != 3 &&
                        currentCell.BottomCell.NewMaterial.Type != 1)
                {
                    if (currentCell.BottomCell.Material.Density < _density)
                    {
                        SwapMaterials(currentCell, currentCell.BottomCell);
                        return;
                    }
                }
            }

        }
    }
}
