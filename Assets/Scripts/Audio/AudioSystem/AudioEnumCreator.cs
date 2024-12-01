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

namespace Audio
{
    [InitializeOnLoad]
    public static class AudioEnumCreator
    {
        private static List<AudioClip> _audioClips;
        private static List<string> _audioNames;
        
        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            OnProjectChanged();
            EditorApplication.projectChanged += OnProjectChanged;
        }


        [MenuItem("Tools/Refresh Audio Enum")]
        private static void OnProjectChanged()
        {
            _ = RefreshGameEventEnum();
        }

        private static async Task RefreshGameEventEnum()
        {
            await LoadAudioClips();
            UpdateAudioEnumFile();
            FillAudioClipHolder();
        }

        private static void FillAudioClipHolder()
        {
            GL_AudioClipHolder audioClipHolder = Object.FindFirstObjectByType<GL_AudioClipHolder>();
            Dictionary<AudioClipEnum, List<AudioClip>> newAudioClipDictionary = new();
            
            foreach (AudioClip audioClip in _audioClips)
            {
                string audioClipName = GetClipName(audioClip);
                if (Enum.TryParse(audioClipName, out AudioClipEnum audioClipEnum))
                {
                    if (!newAudioClipDictionary.TryGetValue(audioClipEnum, out List<AudioClip> audioClipList))
                    {
                        audioClipList = new List<AudioClip>();
                        newAudioClipDictionary[audioClipEnum] = audioClipList;
                    }
                    audioClipList.Add(audioClip);
                }
                else
                {
                    Debug.Log($"Failed to find audio clip enum: {audioClipName}, try to reload domain");
                }
            }
            
            audioClipHolder.SetAudioClips(newAudioClipDictionary);
        }

        private static void UpdateAudioEnumFile()
        {
            var path = Application.dataPath + "/Scripts/Audio/AudioClipEnum.cs";
            HashSet<int> usedIds = new();
            Dictionary<string, int> audioIds = new();
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
                        audioIds[eventName] = eventId;
                        usedIds.Add(eventId);
                    }
                }
            }

            _audioNames = new();
            string enumNames = "";
            foreach (var audioClip in _audioClips)
            {
                string audioName = GetClipName(audioClip);
                int audioId;
                if (audioIds.TryGetValue(audioName, out var id))
                {
                    audioId = id;
                }
                else
                {
                    do
                    {
                        audioId = Random.Range(0, int.MaxValue);
                    } while (usedIds.Contains(audioId));
                    usedIds.Add(audioId);
                    Debug.Log($"new audio clip detected {audioName}, assigned id: {audioId}");
                }
                
                if (_audioNames.Any(name => name.Equals(audioName)))
                {
                    continue;
                }
                
                enumNames += $"{audioName} = {audioId},\n";
                _audioNames.Add(audioName);
            }
            
            enumNames = string.Join("\n", enumNames.Split('\n').OrderBy(x => x));
            
            var content = $@"
            namespace Audio
            {{
                public enum AudioClipEnum
                {{
    {enumNames}
                }}
            }}
            ";
            System.IO.File.WriteAllText(path, content);
        }

        private static string GetClipName(AudioClip audioClip)
        {
            string uselessStuff = audioClip.ToString().Substring(audioClip.ToString().IndexOf('(')-1);
            string eventName = audioClip.ToString().Replace(uselessStuff, "");
            eventName = System.Text.RegularExpressions.Regex.Replace(eventName, @"\d+", "");
            return eventName;
        }

        private static async Task LoadAudioClips()
        {
            _audioClips = new();
            AsyncOperationHandle<IList<AudioClip>> handle = Addressables.LoadAssetsAsync<AudioClip>("Audio", null);
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _audioClips = handle.Result.ToList();
            }
        }
    }


}
#endif