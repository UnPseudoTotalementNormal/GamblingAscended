using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using GameEvents.GameEventDefs;
using UnityEngine;

namespace GameEvents
{
    public class GameEventHolder : MonoBehaviour
    {
        private static GameEventHolder Instance;
        public SerializedDictionary<GameEventEnum, GameEventWithInfo> GameEvents = new();

        public static GameEventWithInfo GetGameEvent(GameEventEnum eventEnum)
        {
            return GetGameEventHolder().GameEvents[eventEnum];
        }

        private static GameEventHolder GetGameEventHolder()
        {
            if (Instance != null)
            {
                return Instance;
            }

            return Instance = FindFirstObjectByType<GameEventHolder>();
        }
        
        public void SetGameEvents(Dictionary<GameEventEnum, GameEventWithInfo> newEvents)
        {
            foreach (var currentEvent in newEvents)
            {
                GameEvents[currentEvent.Key] = currentEvent.Value;
            }
        }
    }
}