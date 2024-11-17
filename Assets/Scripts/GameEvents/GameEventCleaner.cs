using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameEvents;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameEventCleaner : MonoBehaviour
{
    private List<IGameEvent> _gameEvents;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _ = LoadGameEvents();
    }
    
    private async Task LoadGameEvents()
    {
        AsyncOperationHandle<IList<IGameEvent>> handle = Addressables.LoadAssetsAsync<IGameEvent>("GameEvent", null);
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _gameEvents = handle.Result.ToList();
        }
    }

    [ContextMenu("LoadAndClearListeners")]
    private async Task LoadAndClearListeners()
    {
        Debug.Log("Loading GameEvents");
        await LoadGameEvents();
        Debug.Log("GameEvents loaded");
        ClearAllListeners();
        Debug.Log("GameEvents cleared");
    }
    
    private void ClearAllListeners()
    {
        foreach (IGameEvent gameEvent in _gameEvents)
        {
            gameEvent.ClearListeners();
        }
    }
    
    private void OnDestroy()
    {
        ClearAllListeners();
    }
}
