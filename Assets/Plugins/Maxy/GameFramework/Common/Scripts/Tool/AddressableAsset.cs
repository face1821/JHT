using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Maxy.GameFramework.Common.Tool
{
    [Serializable]
    public class AddressableAsset<T> where T : Object
    {
        public enum AddressableAssetState
        {
            None,
            Loading,
            Loaded,
        }

        public AssetReferenceT<T> Asset;
        public AddressableAssetState State { get; private set; }
        public T Result { get => State == AddressableAssetState.Loaded && _handle.IsValid() ? _handle.Result : null; }

        private AsyncOperationHandle<T> _handle;


        public async Task<T> LoadAssetAsync()
        {
            //已加载，且正确，返回结果
            if (State == AddressableAssetState.Loaded && _handle.IsValid())
                return _handle.Result;

            //已加载，但失败，重置状态
            if (State == AddressableAssetState.Loaded && !_handle.IsValid())
                State = AddressableAssetState.None;

            try
            {
                //正在加载，等待
                if (State == AddressableAssetState.Loading)
                {
                    await _handle.Task;
                    return _handle.Result;
                }

                //未加载，开始加载
                State = AddressableAssetState.Loading;
                _handle = Addressables.LoadAssetAsync<T>(Asset);

                await _handle.Task;
                State = AddressableAssetState.Loaded;

                return _handle.Result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Addressable资源加载失败：{e.Message}");

                Addressables.Release(_handle);
                State = AddressableAssetState.None;
                _handle = default;
            }

            return null;
        }

        public async Task ReleaseAssetAsync()
        {
            if (State == AddressableAssetState.Loading)
            {
                await _handle.Task;
            }

            if (State == AddressableAssetState.Loaded && _handle.IsValid())
            {
                Addressables.Release(_handle);

                _handle = default;
                State = AddressableAssetState.None;
            }
        }
    }
}