using GameEvents;
using GameEvents.Enum;
using Interactables.ObjectHolding_Placing.Bases;
using Towers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactables.ObjectHolding_Placing
{
    public class GL_TowerPlaceable : GL_BasePlaceable
    {
        public GL_TowerInfo TowerInfo;
        
        public override void OnPlaced(GameObject spawnedObject)
        {
            base.OnPlaced(spawnedObject);
            var towerModel = Instantiate(TowerInfo.TowerModel, spawnedObject.transform);
            towerModel.transform.localPosition = new Vector3(0, 0, 0);
            GameEventEnum.SetTowerInfo.Invoke(new GameEventTowerInfo { Ids = new[] { spawnedObject.GetGameID() }, TowerInfo = TowerInfo });
        }
    }
}