using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManger : MonoBehaviour
{
    [SerializeField]
    List<ButtonScript> _buttons = new List<ButtonScript>();
    
    private void Awake()
    {
        MapGenerator.OnGameLoaded += OnGameLoaded;
    }

    public void OnGameLoaded(Action<uint> OnMaterialSelected)
    {
        foreach (ButtonScript button in _buttons) 
        { 
            button.Init(OnMaterialSelected); 
        }
    }

    private void OnDestroy()
    {
        MapGenerator.OnGameLoaded -= OnGameLoaded;
    }
}
