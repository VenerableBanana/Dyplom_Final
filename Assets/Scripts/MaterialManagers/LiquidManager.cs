using UnityEngine;

public class LiquidManager : MaterialManager
{
    public LiquidManager() : base()
    {
        _color = Color.blue;
        _density = 1000f;
        _flammable = false;
        _flowDirection = 0;
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


            if (currentCell.BottomCell.NewMaterial.Type == 1 ||
                currentCell.BottomCell.NewMaterial.Type == 2 ||
                currentCell.BottomCell.NewMaterial.Type == 3 ||
                currentCell.BottomCell.NewMaterial.Type == 6 ||
                currentCell.BottomCell.NewMaterial.Type == 7)
            {
                var rng = Random.Range(0f, 2f);

                if (_flowDirection == 0)
                {
                    if (rng >= 1)
                    {
                        if (currentCell.RightCell != null &&
                            currentCell.RightCell.NewMaterial.Type == 0)
                        {
                            if (currentCell.RightBottomCell != null &&
                                currentCell.RightBottomCell.NewMaterial.Type == 0)
                            {
                                currentCell.Material.FlowDirection = 1;
                                MoveToRightBottomCell(currentCell);
                                return;
                            }
                            else
                            {
                                currentCell.Material.FlowDirection = 1;
                                MoveToRightCell(currentCell);
                                return;
                            }
                        }
                        else
                        {
                            if (currentCell.LeftCell != null &&
                                currentCell.LeftCell.NewMaterial.Type == 0)
                            {
                                if (currentCell.LeftBottomCell != null &&
                                    currentCell.LeftBottomCell.NewMaterial.Type == 0)
                                {
                                    currentCell.Material.FlowDirection = -1;
                                    MoveToLeftBottomCell(currentCell);
                                    return;
                                }
                                else
                                {
                                    currentCell.Material.FlowDirection = -1;
                                    MoveToLeftCell(currentCell);
                                    return;
                                }
                            }
                            else
                            {
                                currentCell.IsUpdated = true;
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (currentCell.LeftCell != null &&
                            currentCell.LeftCell.NewMaterial.Type == 0)
                        {
                            if (currentCell.LeftBottomCell != null &&
                                currentCell.LeftBottomCell.NewMaterial.Type == 0)
                            {
                                currentCell.Material.FlowDirection = -1;
                                MoveToLeftBottomCell(currentCell);
                                return;
                            }
                            else
                            {
                                currentCell.Material.FlowDirection = -1;
                                MoveToLeftCell(currentCell);
                                return;
                            }
                        }
                        else
                        {
                            if (currentCell.RightCell != null &&
                                currentCell.RightCell.NewMaterial.Type == 0)
                            {
                                if (currentCell.RightBottomCell != null &&
                                    currentCell.RightBottomCell.NewMaterial.Type == 0)
                                {
                                    currentCell.Material.FlowDirection = 1;
                                    MoveToRightBottomCell(currentCell);
                                    return;
                                }
                                else
                                {
                                    currentCell.Material.FlowDirection = 1;
                                    MoveToRightCell(currentCell);
                                    return;
                                }
                            }
                            else
                            {
                                currentCell.IsUpdated = true;
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (_flowDirection == 1)
                    {
                        if (currentCell.RightCell != null &&
                            currentCell.RightCell.NewMaterial.Type == 0)
                        {
                            if (currentCell.RightBottomCell != null &&
                                currentCell.RightBottomCell.NewMaterial.Type == 0)
                            {
                                MoveToRightBottomCell(currentCell);
                                return;
                            }
                            else
                            {
                                MoveToRightCell(currentCell);
                                return;
                            }
                        }
                        else
                        {
                            currentCell.Material.FlowDirection = -1;
                            return;
                        }
                    }
                    else
                    {
                        if (currentCell.LeftCell != null &&
                            currentCell.LeftCell.NewMaterial.Type == 0)
                        {
                            if (currentCell.LeftBottomCell != null &&
                                currentCell.LeftBottomCell.NewMaterial.Type == 0)
                            {
                                MoveToLeftBottomCell(currentCell);
                                return;
                            }
                            else
                            {
                                MoveToLeftCell(currentCell);
                                return;
                            }
                        }
                        else
                        {
                            currentCell.Material.FlowDirection = 1;
                            return;
                        }
                    }
                }
            }
            else
            {
                bool success;
                MaterialContact(currentCell, currentCell.BottomCell, out success);
                if (success)
                    return;
                MaterialContact(currentCell, currentCell.LeftCell, out success);
                if (success)
                    return;
                MaterialContact(currentCell, currentCell.RightCell, out success);
            }
        }
    }


    private void MaterialContact(Cell currentCell, Cell targetCell, out bool success)
    {
        success = false;
        if (targetCell != null)
        {
            if(targetCell.Material.Type == 8)
            {
                ContactWithLava(currentCell, targetCell);
                success = true;
                return;
            }
            if(targetCell.Material.Type == 9)
            {
                ContactWithFire(currentCell, targetCell);
                success = true;
                return;
            }
        }
    }


    public void ContactWithLava(Cell currentCell, Cell targetCell)
    {
        currentCell.SetMaterial(MapGenerator.Palette[0]);
        targetCell.SetMaterial(MapGenerator.Palette[6]);
    }

    public void ContactWithFire(Cell currentCell, Cell targetCell)
    {
        currentCell.SetMaterial(MapGenerator.Palette[0]);
        targetCell.SetMaterial(MapGenerator.Palette[0]);

    }

}
