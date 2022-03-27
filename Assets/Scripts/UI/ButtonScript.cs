using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    class ButtonScript : MonoBehaviour
    {
        //public GameObject MapGenerator;
        public TextMeshProUGUI text;
        public uint Material;
        private Action<uint> _onMaterialSelected;

        public void Init(Action<uint> OnMaterialSelected)
        {
            _onMaterialSelected = OnMaterialSelected;
            GetComponent<Button>().onClick.AddListener(SetMaterial);
        }
        public void SetMaterial()
        {
            //todo switch na zmiane uintow na opisy materialow

            _onMaterialSelected?.Invoke(Material);
            text.text = Material.ToString();
        }
    }
}
