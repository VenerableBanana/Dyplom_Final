//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using Unity.Entities;
//using UnityEngine;
//using Random = System.Random;

//public class Electricity : MaterialManager
//{

//    private readonly float _framesToDisappear;
//    private float _counter;
//    private bool _stillSpreading;

//    public Electricity()
//    {
//        _color = new Color(0.81f, 0.95f, 1f);
//        _flammable = false;
//        _counter = 0f;
//        _framesToDisappear = 35f;
//        _stillSpreading = true;
//    }

//    public override void CalculatePhysics(Cell currentCell, int xDir, int yDir)
//    {

//        if (_stillSpreading)
//        {
//            List<Cell> directions = new List<Cell>();

//            if (currentCell.BottomCell != null)
//                directions.Add(currentCell.BottomCell);

//            if (currentCell.TopCell != null)
//                directions.Add(currentCell.TopCell);

//            if (currentCell.RightCell != null)
//                directions.Add(currentCell.RightCell);

//            if (currentCell.LeftCell != null)
//                directions.Add(currentCell.LeftCell);

//            if (currentCell.TopRightCell != null)
//                directions.Add(currentCell.TopRightCell);

//            if (currentCell.TopLeftCell != null)
//                directions.Add(currentCell.TopLeftCell);

//            if (currentCell.RightBottomCell != null)
//                directions.Add(currentCell.RightBottomCell);

//            if (currentCell.LeftBottomCell != null)
//                directions.Add(currentCell.LeftBottomCell);

//            foreach (var direction in directions)
//            {
//                if (direction.NewMaterialManager.GetType() == typeof(SolidManager) ||
//                    direction.NewMaterialManager.GetType() == typeof(EmptyManager))
//                {
//                    currentCell.NewMaterialManager = new LiquidManager();
//                    currentCell.IsUpdated = true;
//                    return;
//                }
//            }

//            directions.Clear();

//            if (currentCell.BottomCell != null &&
//                currentCell.BottomCell.NewMaterialManager.GetType() == typeof(LiquidManager))
//                directions.Add(currentCell.BottomCell);

//            if (currentCell.TopCell != null &&
//                currentCell.TopCell.NewMaterialManager.GetType() == typeof(LiquidManager))
//                directions.Add(currentCell.TopCell);

//            if (currentCell.RightCell != null &&
//                currentCell.RightCell.NewMaterialManager.GetType() == typeof(LiquidManager))
//                directions.Add(currentCell.RightCell);

//            if (currentCell.LeftCell != null &&
//                currentCell.LeftCell.NewMaterialManager.GetType() == typeof(LiquidManager))
//                directions.Add(currentCell.LeftCell);

//            if (currentCell.TopRightCell != null &&
//                currentCell.TopRightCell.NewMaterialManager.GetType() == typeof(LiquidManager))
//                directions.Add(currentCell.TopRightCell);

//            if (currentCell.TopLeftCell != null &&
//                currentCell.TopLeftCell.NewMaterialManager.GetType() == typeof(LiquidManager))
//                directions.Add(currentCell.TopLeftCell);

//            if (currentCell.RightBottomCell != null &&
//                currentCell.RightBottomCell.NewMaterialManager.GetType() == typeof(LiquidManager))
//                directions.Add(currentCell.RightBottomCell);

//            if (currentCell.LeftBottomCell != null &&
//                currentCell.LeftBottomCell.NewMaterialManager.GetType() == typeof(LiquidManager))
//                directions.Add(currentCell.LeftBottomCell);

//            if (directions.Count == 0)
//            {
//                _counter++;
//            }
//            else
//            {
//                Random rnd = new Random();
//                int index = rnd.Next(0, directions.Count - 1);

//                directions[index].NewMaterialManager = new Electricity();
//                directions[index].IsUpdated = true;
//                _stillSpreading = false;
//                _counter++;
//            }

//        }
//        else
//        {
//            _counter++;

//            if (_counter >= _framesToDisappear)
//            {
//                currentCell.NewMaterialManager = new LiquidManager();
//                currentCell.IsUpdated = true;
//            }
//        }
//    }
//}
