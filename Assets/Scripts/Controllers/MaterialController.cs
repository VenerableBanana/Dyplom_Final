using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditorInternal.VR;
using UnityEngine;

namespace Assets.Scripts.Materials
{
    class MaterialController
    {
        private LiquidManager _liquidManager;
        private EmptyManager _emptyManager;
        private SolidManager _solidManager;
        private SandManager _sandManager;
        private SmokeManager _smokeManager;
        private SteamManager _steamManager;
        private ObsidianManager _obsidianManager;
        private WoodManager _woodManager;
        private LavaManager _lavaManager;
        private FireManager _fireManager;
        //todo reszta materialow

        public MaterialController()
        {
            _liquidManager = new LiquidManager();
            _emptyManager = new EmptyManager();
            _solidManager = new SolidManager();
            _sandManager = new SandManager();
            _smokeManager = new SmokeManager();
            _steamManager = new SteamManager();
            _obsidianManager = new ObsidianManager();
            _woodManager = new WoodManager();
            _lavaManager = new LavaManager();
            _fireManager = new FireManager();
            //todo reszta materialow
        }

        public void CalculateMaterialPhysics(Cell cell)
        {
            if (cell.IsUpdated)
                return;

            switch (cell.Material.Type)
            {
                case 0:
                    _emptyManager.CalculatePhysics(cell);
                    break;
                case 1:
                    _solidManager.CalculatePhysics(cell);
                    break;
                case 2:
                    _liquidManager.CalculatePhysics(cell);
                    break;
                case 3:
                    _sandManager.CalculatePhysics(cell);
                    break;
                case 4:
                    _smokeManager.CalculatePhysics(cell);
                    break;
                case 5:
                    _steamManager.CalculatePhysics(cell);
                    break;
                case 6:
                    _obsidianManager.CalculatePhysics(cell);
                    break;
                case 7:
                    _woodManager.CalculatePhysics(cell);
                    break;
                case 8:
                    _lavaManager.CalculatePhysics(cell);
                    break;
                case 9:
                    _fireManager.CalculatePhysics(cell);
                    break;
                case 10:
                    break;
                case 11:
                    break;
                default:
                    break;
            }
        }

        public Color GetColor(Cell cell)
        {
            return cell.NewMaterial.Color;
        }
    }
}
