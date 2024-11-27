#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameEvents;
using GameEvents.GameEventDefs;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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
    }

    private static void FillGameEventHolder()
    {
        GameEventHolder gameEventHolder = Object.FindFirstObjectByType<GameEventHolder>();
        Dictionary<GameEventEnum, GameEventWithInfo> newGameEventDictionary = new();
        
        foreach (IGameEvent gameEvent in _gameEvents)
        {
            string eventName = GetEventName(gameEvent);
            if (Enum.TryParse(eventName, out GameEventEnum gameEventEnum))
            {
                newGameEventDictionary[gameEventEnum] = (GameEventWithInfo)gameEvent;
            }
            else
            {
                Debug.Log($"Failed to find event enum: {eventName}, try to reload domain");
            }
        }
        
        gameEventHolder.SetGameEvents(newGameEventDictionary);
    }

    private static void UpdateEnumFile()
    {
        var path = Application.dataPath + "/Scripts/GameEvents/GameEventEnum.cs";
        HashSet<int> usedIds = new();
        Dictionary<string, int> eventIds = new();
        if (!System.IO.File.Exists(path))
        {
            var directory = System.IO.Path.GetDirectoryName(path);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            System.IO.File.Create(path).Dispose();
        }
        else
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string line in lines)
            {
                if (line.Contains("="))
                {
                    string eventName = line.Substring(0, line.IndexOf('=') - 1);
                    string eventIdString = line.Substring(line.IndexOf('=') + 2).Replace(",", "");
                    int eventId = int.Parse(eventIdString);
                    eventIds[eventName] = eventId;
                    usedIds.Add(eventId);
                }
            }
        }

        _eventNames = new();
        string enumNames = "";
        foreach (var gameEvent in _gameEvents)
        {
            string eventName = GetEventName(gameEvent);
            int eventId;
            if (eventIds.TryGetValue(eventName, out var id))
            {
                eventId = id;
            }
            else
            {
                do
                {
                    eventId = Random.Range(0, int.MaxValue);
                } while (usedIds.Contains(eventId));
                usedIds.Add(eventId);
                Debug.Log($"new Game Event detected {eventName}, assigned id: {eventId}");
            }
            
            enumNames += $"{eventName} = {eventId},\n";
            _eventNames.Add(eventName);
        }
        
        enumNames = string.Join("\n", enumNames.Split('\n').OrderBy(x => x));
        
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
