using System.Linq;
using UnityEngine;

namespace GameEvents
{
    public static class GameObjectGameIDExtension
    {
        public static int GetGameID(this GameObject gameObject) => GameID.GetGameID(gameObject);
        
        public static bool HasGameID(this GameObject gameObject, int[] gameIDs) => gameIDs.Contains(gameObject.GetGameID());
    }
}