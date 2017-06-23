using UnityEngine;
using System.IO;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class IPathTools {
    /// <summary>
    /// 获取平台文件夹名
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    public static string GetPlatformFolderName(RuntimePlatform platform) {
        switch (platform) {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "IOS";
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return "Windows";
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                return "OSX";
            default:
                return null;
        }

    }

    public static string GetAppFilePath() {
        string path = "";
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
            path = Application.streamingAssetsPath;
        } else {
            path = Application.persistentDataPath;
        }

        return path;
    }

    public static string GetAssetBundlePath() {
        string platFolder = GetPlatformFolderName(Application.platform);
        string allPath = Path.Combine(GetAppFilePath(), platFolder);
        allPath = Path.Combine(allPath, "AssetBundles");
        return allPath;
    }

   
}

