using UnityEngine;
using System.IO;

public class Test : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== ES3 存储诊断 ===");
        Debug.Log("persistentDataPath: " + Application.persistentDataPath);
        Debug.Log("installMode: " + Application.installMode);
        Debug.Log("platform: " + Application.platform);

        // 测试原生文件写入
        string testPath = Path.Combine(Application.persistentDataPath, "native_test.txt");
        try
        {
            File.WriteAllText(testPath, "hello from native");
            Debug.Log("✅ 原生写入成功: " + testPath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ 原生写入失败: " + e);
        }

        // 测试 ES3 写入
        try
        {
            ES3.Save("test_key", "hello from es3", "SaveFile.es3");
            string val = ES3.Load<string>("test_key", "SaveFile.es3");
            Debug.Log("✅ ES3 读写成功: " + val);
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ ES3 写入失败: " + e);
        }
    }
}