using UnityEditor;
using UnityEngine;

namespace Maxy.GameFramework.Common.Editor
{
    public static class AutoAddTags
    {
        // 在这里写你包用到的所有自定义 Tag
        private static readonly string[] RequiredTags =
        {
            "SystemList",
        };

        [InitializeOnLoadMethod]
        private static void AddMissingTags()
        {
            // 加载 TagManager.asset
            var tagManagerAsset = AssetDatabase.LoadAssetAtPath<Object>("ProjectSettings/TagManager.asset");
            if (tagManagerAsset == null)
            {
                Debug.LogWarning("无法找到TagManager.asset，跳过自动添加Tag");
                return;
            }

            SerializedObject tagManager = new SerializedObject(tagManagerAsset);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            foreach (var tag in RequiredTags)
            {
                bool exists = false;
                for (int i = 0; i < tagsProp.arraySize; i++)
                {
                    SerializedProperty tagProp = tagsProp.GetArrayElementAtIndex(i);
                    if (tagProp.stringValue == tag)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
                    SerializedProperty newTagProp = tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1);
                    newTagProp.stringValue = tag;
                    Debug.Log($"[AutoAddTags]已自动添加Tag：{tag}");
                }
            }

            tagManager.ApplyModifiedProperties();
            EditorUtility.SetDirty(tagManagerAsset);
            AssetDatabase.SaveAssets();
        }
    }
}