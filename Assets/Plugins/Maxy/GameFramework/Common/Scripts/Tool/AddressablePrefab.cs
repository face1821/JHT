using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Maxy.GameFramework.Common.Tool
{
     /// <summary>
        /// 自动化管理Addressable的GameObject的Prefab资源，支持多次实例化和统一销毁
        /// 请务必使用这里的Destroy方法，而不是直接调用Unity的Destroy
        /// </summary>
        [Serializable]

    public class AddressablePrefab
    {
        public enum AddressablePrefabState
        {
            None,
            Loading,
            Loaded,
        }

        [SerializeField]
        public AssetReferenceT<GameObject> Prefab;

        public AddressablePrefabState State { get; private set; }
        public int InstanceCount => _instanceList.Count;
        public IReadOnlyList<GameObject> InstanceList => _instanceList;
        private List<GameObject> _instanceList = new List<GameObject>();
        private AsyncOperationHandle<GameObject> _handle;

        public async Task<GameObject> InstantiateAsync(Transform parent = null)
        {
            //已加载，且正确，直接实例化
            if (State == AddressablePrefabState.Loaded && _handle.IsValid())
            {
                return CreateInstance(parent);
            }

            //已加载，但失败，重置状态
            if (State == AddressablePrefabState.Loaded && !_handle.IsValid())
                State = AddressablePrefabState.None;

            try
            {
                //正在加载，等待
                if (State == AddressablePrefabState.Loading)
                {
                    await _handle.Task;
                    return CreateInstance(parent);
                }

                //未加载，开始加载
                State = AddressablePrefabState.Loading;
                _handle = Prefab.LoadAssetAsync();

                await _handle.Task;
                State = AddressablePrefabState.Loaded;

                return CreateInstance(parent);
            }
            catch (Exception e)
            {
                Console.WriteLine($"AddressablePrefab加载失败：{e.Message}");

                if (_handle.IsValid())
                    Addressables.Release(_handle);

                State = AddressablePrefabState.None;
                _handle = default;
            }

            return null;
        }

        private GameObject CreateInstance(Transform parent)
        {
            var instance = GameObject.Instantiate(_handle.Result, parent);
            _instanceList.Add(instance);
            return instance;
        }

        public void Destroy(GameObject instance)
        {
            if (State != AddressablePrefabState.Loaded) return;

            if (instance == null) return;

            if (_instanceList.Remove(instance))
            {
                GameObject.Destroy(instance);
            }

            TryReleaseAsset();
        }

        public void DestroyAll()
        {
            if (State != AddressablePrefabState.Loaded) return;

            foreach (var instance in _instanceList)
            {
                if (instance != null)
                    GameObject.Destroy(instance);
            }

            _instanceList.Clear();

            TryReleaseAsset();
        }

        public void Refresh()
        {
            if (State != AddressablePrefabState.Loaded) return;
            if (_instanceList.Count == 0) return;

            for (int i = _instanceList.Count - 1; i >= 0; i--)
            {
                if (_instanceList[i] == null)
                {
                    _instanceList.RemoveAt(i);
                }
            }

            TryReleaseAsset();
        }

        private void TryReleaseAsset()
        {
            if (_instanceList.Count == 0)
            {
                Addressables.Release(_handle);
                _handle = default;
                State = AddressablePrefabState.None;
            }
        }

        public async Task ReleaseAssetAsync()
        {
            if (State == AddressablePrefabState.Loading)
            {
                await _handle.Task;
            }

            DestroyAll();
        }
    }
}