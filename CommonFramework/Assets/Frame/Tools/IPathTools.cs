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
        return Path.Combine(GetAppFilePath(), platFolder);
    }

    public static string GetWWWAssetBundlePath() {
        string tmpStr;

        if (Application.platform==RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
            tmpStr = "file:///" + GetAssetBundlePath();
        } else {
            tmpStr = GetAssetBundlePath();

#if UNITY_ANDROID
        tmpStr = "jar:file://" + tmpStr;
#elif UNITY_STANDALONE_WIN
            tmpStr = "file:///" + tmpStr;
#else
            tmpStr = "file:///" + tmpStr;
#endif
        }

        return tmpStr;
    }


}

