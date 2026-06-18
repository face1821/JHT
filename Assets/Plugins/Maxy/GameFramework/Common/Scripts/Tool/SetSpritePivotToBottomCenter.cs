using System.IO;
using UnityEditor;
using UnityEngine;

namespace Maxy.GameFramework.Common.Tool
{
    /// <summary>
    /// 右键文件，设置所选文件的精灵图们的轴心为底部中心
    /// </summary>
    public static class SetSpritePivotToBottomCenter
    {
        // 注册右键菜单：priority值越小，菜单位置越靠上（默认1000即可）
        [MenuItem("Assets/Set Pivot To Bottom Center", false, 0)]
        private static void LogSelectedFilePath()
        {
            // 获取Project窗口中选中的单个文件/文件夹的工程相对路径
            string selectedAssetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            var path = Path.GetFullPath(selectedAssetPath);

            // 判空：防止未选中任何对象时触发（理论上右键菜单仅选中对象时显示）
            if (string.IsNullOrEmpty(selectedAssetPath) || !File.Exists(path))
            {
                Debug.LogWarning("No file selected, unable to obtain path!");
                return;
            }

            //获取meta文件
            path += ".meta";

            using var reader = new StreamReader(path);
            string content = reader.ReadToEnd();

            content = content.Replace("alignment: 0", "alignment: 9");
            content = content.Replace("pivot: {x: 0, y: 0}", "pivot: {x: 0.5, y: 0}");
            content = content.Replace("pivot: {x: 0.5, y: 0.5}", "pivot: {x: 0.5, y: 0}");
            reader.Close();

            File.WriteAllText(path, content);
            AssetDatabase.ImportAsset(selectedAssetPath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.Refresh();
        }

        // 控制右键菜单是否可用：仅选中有效对象时显示菜单（可选但推荐）
        [MenuItem("Assets/Set Pivot To Bottom Center", true, 0)]
        private static bool ValidateLogFilePath()
        {
            // 仅当Project窗口选中了至少一个对象时，菜单才可用
            return Selection.activeObject != null;
        }
    }
}