using UnityEngine;

namespace GameEvents.GameEventDefs
{
    [CreateAssetMenu(fileName = "GameEventNone", menuName = "GameEvents/GameEventNone")]
    public class GameEventNone : GameEventWithInfo
    {
        public override void Invoke(GameEventInfo value)
        {
            Debug.Log("none :D");
        }
    }
}