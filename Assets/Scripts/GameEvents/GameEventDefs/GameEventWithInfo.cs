using UnityEngine;

namespace GameEvents.GameEventDefs
{
    [CreateAssetMenu(fileName = "GameEventWithInfo", menuName = "GameEvents/GameEventWithInfo")]
    public class GameEventWithInfo : GameEvent<GameEventInfo> {}
}