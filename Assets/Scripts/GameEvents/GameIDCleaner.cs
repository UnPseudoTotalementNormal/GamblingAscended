using System;
using UnityEngine;

namespace GameEvents
{
    public class GameIDCleaner : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            CleanIds();
        }

        [ContextMenu("CleanIds")]
        private void CleanIds()
        {
            GameID.CleanIds();
            Debug.Log("GameIDs cleaned");
        }
    }
}