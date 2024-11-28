using Interactables.ObjectHolding_Placing.Bases;
using Towers;
using UnityEngine;

namespace Interactables.ObjectHolding_Placing
{
    public class GL_TowerPlaceable : GL_BasePlaceable
    {
        [SerializeField] private GL_TowerObject _towerObject;
        
        public override void OnPlaced(GameObject spawnedObject)
        {
            base.OnPlaced(spawnedObject);
        }
    }
}