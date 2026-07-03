using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Maxy.GameFramework.Common.System
{
    /// <summary>
    /// 日志输出系统，完全是静态类，不依赖系统中心
    /// </summary>
    public static class MLogger
    {
        [Conditional("UNITY_EDITOR")]
        public static void Log(object message, Object ctx = null) { Debug.Log(message, ctx); }

        [Conditional("UNITY_EDITOR")]
        public static void LogWarning(object message, Object ctx = null) { Debug.LogWarning(message, ctx); }

        [Conditional("UNITY_EDITOR")]
        public static void LogError(object message, Object ctx = null) { Debug.LogError(message, ctx); }
    }
}