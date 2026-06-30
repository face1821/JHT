using System.Collections.Generic;
using Maxy.GameFramework.Common.System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.LevelChunk
{
    public class LevelChunkManager : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] public List<string> LoadedChunkList { get; private set; } = new List<string>();

        /// <summary>
        /// 加载指定的区块场景
        /// </summary>
        /// <param name="levelChunk"></param>
        public void LoadChunk(string levelChunk)
        {
            MLogger.Log($"区块系统：加载 {levelChunk}");
            SceneManager.LoadSceneAsync(levelChunk, LoadSceneMode.Additive);
            LoadedChunkList.Add(levelChunk);
        }

        /// <summary>
        /// 卸载指定的区块场景
        /// </summary>
        /// <param name="levelChunk"></param>
        public void UnloadChunk(string levelChunk)
        {
            MLogger.Log($"区块系统：卸载 {levelChunk}");
            SceneManager.UnloadSceneAsync(levelChunk);
            LoadedChunkList.Remove(levelChunk);
        }
    }
}