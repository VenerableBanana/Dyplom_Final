using UnityEngine;

public class SmokeManager : MaterialManager
{
    private readonly float _framesToDisappear;
    private int _flowDirection;

    public SmokeManager() : base()
    {
        _color = new Color(0f, 0f, 0f, 0.6f);
        _framesToDisappear = 200f;
        _density = 1.2f;
        _flammable = false;
        _flowDirection = 0;
    }

    public override void CalculatePhysics(Cell currentCell)
    {
        if (currentCell.Material.Counter <= _framesToDisappear)
        {
            currentCell.Material.Counter++;

            if (currentCell.TopCell != null)
            {
                if (currentCell.TopCell.NewMaterial.Type == 0)
                {
                    MoveToTopCell(currentCell);
                    return;
                }


                if (currentCell.TopCell.NewMaterial.Type != 0)
                {
                    var rng = Random.Range(0f, 2f);

                    if (_flowDirection == 0)
                    {
                        if (rng >= 1)
                        {
                            if (currentCell.RightCell != null && currentCell.RightCell.NewMaterial.Type == 0)
                            {
                                if (currentCell.TopRightCell != null &&
                                    currentCell.TopRightCell.NewMaterial.Type == 0)
                                {
                                    currentCell.Material.FlowDirection = 1;
                                    MoveToTopRightCell(currentCell);
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
                                if (currentCell.LeftCell != null && currentCell.LeftCell.NewMaterial.Type == 0)
                                {
                                    if (currentCell.TopLeftCell != null &&
                                        currentCell.TopLeftCell.NewMaterial.Type == 0)
                                    {
                                        currentCell.Material.FlowDirection = -1;
                                        MoveToTopLeftCell(currentCell);
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
                                    currentCell.SetMaterial(currentCell.Material);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (currentCell.LeftCell != null && currentCell.LeftCell.NewMaterial.Type == 0)
                            {
                                if (currentCell.TopLeftCell != null &&
                                    currentCell.TopLeftCell.NewMaterial.Type == 0)
                                {
                                    currentCell.Material.FlowDirection = -1;
                                    MoveToTopLeftCell(currentCell);
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
                                if (currentCell.RightCell != null && currentCell.RightCell.NewMaterial.Type == 0)
                                {
                                    if (currentCell.TopRightCell != null &&
                                        currentCell.TopRightCell.NewMaterial.Type == 0)
                                    {
                                        currentCell.Material.FlowDirection = 1;
                                        MoveToTopRightCell(currentCell);
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
                                    currentCell.SetMaterial(currentCell.Material);
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
                                if (currentCell.TopRightCell != null &&
                                    currentCell.TopRightCell.NewMaterial.Type == 0)
                                {
                                    MoveToTopRightCell(currentCell);
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
                                currentCell.SetMaterial(currentCell.Material);
                                return;
                            }
                        }
                        else
                        {
                            if (currentCell.LeftCell != null &&
                                currentCell.LeftCell.NewMaterial.Type == 0)
                            {
                                if (currentCell.TopLeftCell != null &&
                                    currentCell.TopLeftCell.NewMaterial.Type == 0)
                                {
                                    MoveToTopLeftCell(currentCell);
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
                                currentCell.SetMaterial(currentCell.Material);
                                return;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            currentCell.SetMaterial(MapGenerator.Palette[0]);
            return;
        }
    }
}
