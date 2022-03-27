using UnityEngine;
using Random = UnityEngine.Random;

public class LavaManager : MaterialManager
{

    private int _counter;
    private int _flowDirection;

    public LavaManager()
    {
        _color = new Color(0.5f, 0, 0f);
        _density = 1000f;
        _flammable = false;
        _counter = 0;
        _flowDirection = 0;
    }

    public override void CalculatePhysics(Cell currentCell)
    {
        //Jeœli nad nami jest pusty Cell i trafiliœmy w 0,4% szansy, to wypuszczamy nad siebie dym
        if (currentCell.TopCell != null &&
            currentCell.TopCell.NewMaterial.Type == 0)
        {
            var rng = Random.Range(0f, 100f);

            if (rng <= 0.4f)
            {
                currentCell.TopCell.SetMaterial(MapGenerator.Palette[4]);
            }
        }

        //Jeœli pod spodem jest woda to zamieniamy j¹ na obsydian
        if (currentCell.BottomCell != null &&
            currentCell.BottomCell.NewMaterial.Type == 2)
        {
            ContactWithLiquid(currentCell);
            return;
        }

        //Co 4 klatki wykonujemy ruch, ¿eby wydawa³o siê, ¿e lawa jest wolniejsza
        if (currentCell.Material.Counter == 4)
        {

            currentCell.Material.Counter = 0;

            if (currentCell.BottomCell != null)
            {
                if (currentCell.BottomCell.NewMaterial.Type == 0)
                {
                    MoveToBottomCell(currentCell);
                    return;
                }


                if (!currentCell.BottomCell.NewMaterial.Flammable)
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

                                currentCell.SetMaterial(currentCell.Material);
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

                                currentCell.SetMaterial(currentCell.Material);
                                return;

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
                                currentCell.SetMaterial(currentCell.Material);
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
                                currentCell.SetMaterial(currentCell.Material);
                                return;
                            }
                        }
                    }
                }
                else //jeœli materia³ da siê zapaliæ
                {

                }
            }
        }
        else
        {
            currentCell.Material.Counter++;
            currentCell.SetMaterial(currentCell.Material);
            return;
        }

    }

    public void ContactWithLiquid(Cell currentCell)
    {
        currentCell.SetMaterial(MapGenerator.Palette[0]);
        currentCell.BottomCell.SetMaterial(MapGenerator.Palette[6]);
        currentCell.IsUpdated = true;
        currentCell.BottomCell.IsUpdated = true;
    }
}
