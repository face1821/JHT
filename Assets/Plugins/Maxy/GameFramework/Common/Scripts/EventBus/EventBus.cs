using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

namespace Maxy.GameFramework.Common.Events
{
    public static class EventBus
    {
        private static Dictionary<Type, HashSet<Delegate>> _eventSubscriptions = new Dictionary<Type, HashSet<Delegate>>();

        public static void Subscribe<T>(Action<T> onEvent) { SubscribeEvent(onEvent); }

        public static void Unsubscribe<T>(Action<T> onEvent) { UnsubscribeEvent(onEvent); }

        public static void Publish<T>(T eventData) { PublishEvent(eventData); }

        private static void SubscribeEvent<T>(Action<T> onEvent)
        {
            Type eventType = typeof(T);

            // 获取该事件类型的订阅列表（不存在则创建）
            if (!_eventSubscriptions.TryGetValue(eventType, out var _))
            {
                var list = new HashSet<Delegate>();
                _eventSubscriptions[eventType] = list;
            }

            var subscriptions = _eventSubscriptions[eventType];
            // 避免重复订阅
            if (subscriptions.Contains(onEvent))
            {
                Debug.Log($"订阅者{onEvent.Method.Name}已订阅事件{eventType.Name}，无需重复订阅");
            }

            subscriptions.Add(onEvent);
        }

        private static void UnsubscribeEvent<T>(Action<T> onEvent)
        {
            Type eventType = typeof(T);

            if (!_eventSubscriptions.TryGetValue(eventType, out var subscriptions)) return;

            // 移除匹配的订阅
            subscriptions.Remove(onEvent);

            // 无订阅时清理字典（节省内存）
            if (subscriptions.Count == 0)
            {
                _eventSubscriptions.Remove(eventType);
            }
        }

        private static void PublishEvent<T>(T eventData)
        {
            Type eventType = typeof(T);

            if (!_eventSubscriptions.TryGetValue(eventType, out var subscriptions)) return;

            foreach (var subscription in subscriptions)
            {
                if (!IsValidEvent(subscription)) continue;

                ((Action<T>)subscription).Invoke(eventData);
            }
        }

        private static bool IsValidEvent(Delegate onEvent)
        {
            if (onEvent == null) return false;
            if (onEvent.Target == null) return false;
            if (onEvent.Target is MonoBehaviour mono)
            {
                // Unity内置校验：是否被Destroy、gameObject是否存在
                return mono != null && mono.gameObject != null && !mono.gameObject.IsDestroyed();
            }

            return true;
        }
    }
}