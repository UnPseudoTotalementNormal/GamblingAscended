using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Breezorio
{
    public static class AddressablesManager
    {
        private static Dictionary<string, Object> _preloadedAssets = new();
        
        private static bool _isInitialized = false;

        public static async Task Initialize()
        {
            _preloadedAssets = new Dictionary<string, Object>();
            _isInitialized = true;

            try
            {
                AsyncOperationHandle<IList<Object>> handle = Addressables.LoadAssetsAsync<Object>("Preloaded", null);
                await handle.Task;
                await OnGroupLoaded(handle);
            }
            catch
            {
                Debug.Log("No preloaded group found.");
            }
        }
        
        private static async Task OnGroupLoaded(AsyncOperationHandle<IList<Object>> handle)
        {
            int preloadedObjects = 0;
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("Failed to preload group.");
                return;
            }
            
            foreach (Object asset in handle.Result)
            {
                if (_preloadedAssets.ContainsKey(asset.name)) continue;
                
                string address = await GetObjectAddress(asset);
                
                _preloadedAssets.Add(address, asset);
                preloadedObjects += 1;
                Debug.Log($"Preloaded {asset.name} at address {address}");
            }

            Debug.Log($"Group preloaded successfully with {preloadedObjects} objects!");
        }

        private static async Task<string> GetObjectAddress(Object asset)
        {
            foreach (IResourceLocator locator in Addressables.ResourceLocators)
            {
                foreach (object key in locator.Keys)
                {
                    Object objectCheck = await Addressables.LoadAssetAsync<Object>(key).Task;

                    if (objectCheck != asset) continue;
                    
                    return key.ToString();
                }
            }
            return null;
        }
        
        public static T GetPreloadedAsset<T>(string assetAddress) where T : UnityEngine.Object
        {
            if (_preloadedAssets.TryGetValue(assetAddress, out Object asset) && asset is T)
            {
                return (T)asset;
            }
            //Debug.LogWarning($"Asset {assetAddress} not found in preloaded assets.");
            return null;
        }
        
        public static async Task<T> LoadAssetAsync<T>(string assetAddress) where T : UnityEngine.Object
        {
            if (!_isInitialized) await Initialize();
            
            T preloadedAsset = GetPreloadedAsset<T>(assetAddress);
            
            return preloadedAsset ?? await Addressables.LoadAssetAsync<T>(assetAddress).Task;
        }

        public static async Task WaitForPreloadedAsset()
        {
            while (!_isInitialized)
            {
                await Task.Yield();
            }
        }
    }
}