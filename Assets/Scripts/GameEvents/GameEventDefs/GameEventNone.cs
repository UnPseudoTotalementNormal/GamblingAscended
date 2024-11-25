using UnityEngine;

namespace GameEvents.GameEventDefs
{
    [CreateAssetMenu(fileName = "GameEventNone", menuName = "GameEvents/GameEventNone")]
    public class GameEventNone : GameEvent<GameEventInfo>
    {
        public override void Invoke(GameEventInfo value)
        {
            
        }
    }
}