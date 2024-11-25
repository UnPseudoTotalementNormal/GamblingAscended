#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameEvents;
using GameEvents.GameEventDefs;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[InitializeOnLoad]
public static class GameEventEnumCreator
{
    private static List<IGameEvent> _gameEvents;
    private static List<string> _eventNames;
    
    [InitializeOnLoadMethod]
    public static void Initialize()
    {
        OnProjectChanged();
        EditorApplication.projectChanged += OnProjectChanged;
    }


    private static void OnProjectChanged()
    {
        _ = RefreshGameEventEnum();
    }

    private static async Task RefreshGameEventEnum()
    {
        await LoadGameEvents();
        UpdateEnumFile();
        FillGameEventHolder();
        Debug.Log("refreshed game event enum");
    }

    private static void FillGameEventHolder()
    {
        GameEventHolder gameEventHolder = Object.FindFirstObjectByType<GameEventHolder>();
        Dictionary<GameEventEnum, GameEventWithInfo> newGameEventDictionary = new();
        foreach (IGameEvent gameEvent in _gameEvents)
        {
            string eventName = GetEventName(gameEvent);
            var gameEventEnum = System.Enum.Parse(typeof(GameEventEnum), eventName);
            newGameEventDictionary[(GameEventEnum)gameEventEnum] = (GameEventWithInfo)gameEvent;
        }
        
        gameEventHolder.SetGameEvents(newGameEventDictionary);
    }

    private static void UpdateEnumFile()
    {
        var path = Application.dataPath + "/Scripts/GameEvents/GameEventEnum.cs";
        if (!System.IO.File.Exists(path))
        {
            var directory = System.IO.Path.GetDirectoryName(path);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            System.IO.File.Create(path).Dispose();
        }

        _eventNames = new();
        string enumNames = "";
        foreach (var gameEvent in _gameEvents)
        {
            var eventName = GetEventName(gameEvent);
            enumNames += $"{eventName},\n";
            _eventNames.Add(eventName);
        }
        var content = $@"
    namespace GameEvents
    {{
        public enum GameEventEnum
        {{
            {enumNames}
        }}
    }}
    ";
        System.IO.File.WriteAllText(path, content);
    }

    private static string GetEventName(IGameEvent gameEvent)
    {
        string uselessStuff = gameEvent.ToString().Substring(gameEvent.ToString().IndexOf('(')-1);
        string eventName = gameEvent.ToString().Replace(uselessStuff, "");
        return eventName;
    }

    private static async Task LoadGameEvents()
    {
        _gameEvents = new();
        AsyncOperationHandle<IList<IGameEvent>> handle = Addressables.LoadAssetsAsync<IGameEvent>("GameEvent", null);
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _gameEvents = handle.Result.ToList();
        }
    }
}
#endif
