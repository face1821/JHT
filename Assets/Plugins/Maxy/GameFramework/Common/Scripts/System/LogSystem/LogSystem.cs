using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Maxy.GameFramework.Common.System
{
    /// <summary>
    /// 日志输出系统，完全是静态类，不依赖系统中心
    /// </summary>
    public static class LogSystem
    {
        [Conditional("UNITY_EDITOR")]
        public static void Log(object message) { Debug.Log(message); }

        [Conditional("UNITY_EDITOR")]
        public static void LogWarning(object message) { Debug.LogWarning(message); }

        [Conditional("UNITY_EDITOR")]
        public static void LogError(object message) { Debug.LogError(message); }
    }
}