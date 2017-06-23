using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class AssetBundleEditor {

    [MenuItem("Itools/BuildAssetBundle")]
    public static void BuildeAssetBundle() {
        string outPath = IPathTools.GetAssetBundlePath();

        BuildPipeline.BuildAssetBundles(outPath, 0, EditorUserBuildSettings.activeBuildTarget);

        AssetDatabase.Refresh();
    }

    [MenuItem("Itools/MarkAssetBundle")]
    public static void MarkAssetBundle() {
        AssetDatabase.RemoveUnusedAssetBundleNames();

        string path = Application.dataPath + "/Art/Scenes/";

        DirectoryInfo dirInfo = new DirectoryInfo(path);

        // 遍历路径下的所有文件
        FileSystemInfo[] fileInfos = dirInfo.GetFileSystemInfos();
        for (int i = 0; i < fileInfos.Length; i++) {
            FileSystemInfo tempInfo = fileInfos[i];
            if (tempInfo is DirectoryInfo) {
                string tempPath = Path.Combine(path, tempInfo.Name);
                SceneOverView(tempPath);
            }
        }

        AssetDatabase.Refresh();
    }

    public static string FixedWindowsPath(string path) {
        path = path.Replace("\\", "/");

        return path;
    }

    // 对整个场景文件夹进行遍历
    private static void SceneOverView(string scenePath) {
        // 这里用读写字符串的方式，应该改成读写二进制
        string textFileName = "Record.txt";
        string tempPath = scenePath + textFileName;
        FileStream fs = new FileStream(tempPath, FileMode.OpenOrCreate);
        StreamWriter bw = new StreamWriter(fs);

        // 存储对应关系
        Dictionary<string, string> readDic = new Dictionary<string, string>();

        ChangeHead(scenePath, readDic);

        // 第一行写入总行数
        bw.WriteLine(readDic.Count);

        foreach (string key in readDic.Keys) {
            bw.Write(key);
            bw.Write(" ");
            bw.Write(readDic[key]);
            bw.Write("\n");
        }

        bw.Close();
        fs.Close();
    }

    // 截取相对路径
    private static void ChangeHead(string fullPath, Dictionary<string, string> theWriter) {

        // 截取Assets 之后的路径
        int index = fullPath.IndexOf("Assets");
        string replacePath = fullPath.Substring(index, fullPath.Length - index);
        DirectoryInfo dir = new DirectoryInfo(fullPath);

        if (dir != null) {
            ListFiles(dir, replacePath, theWriter);
        } else {
            Debug.LogWarning("this path is not exist");
        }
    }

    // 遍历场景中每一个功能文件夹
    private static void ListFiles(FileSystemInfo info, string replacePath, Dictionary<string, string> theWriter) {
        if (!info.Exists) {
            Debug.LogWarning("is not exist");
            return;
        }

        DirectoryInfo dir = info as DirectoryInfo;
        FileSystemInfo[] files = dir.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++) {
            // 强制转换为fileInfo来判断是否是文件
            FileInfo file = files[i] as FileInfo;
            if (file != null) {  // 文件的操作
                ChangeMark(file, replacePath, theWriter);
            } else {            // 目录的操作
                ListFiles(files[i], replacePath, theWriter);
            }
        }
    }

    // 改变物体的tag
    private static void ChangeMark(FileInfo tmpFile, string replacePath, Dictionary<string, string> theWriter) {
        if (tmpFile.Extension == ".meta") {
            return;
        }

        string markStr = GetBundlePath(tmpFile, replacePath);
        Debug.Log("markStr == " + markStr);
        ChangeAssetMark(tmpFile, markStr, theWriter);
    }

    // 计算mart标记值等于多少
    private static string GetBundlePath(FileInfo file, string replacePath) {

        string path = FixedWindowsPath(file.FullName);

        int nameCount = path.LastIndexOf(file.Name);
        int tmpCount = replacePath.LastIndexOf("/");
        string sceneHead = replacePath.Substring(tmpCount + 1, replacePath.Length - tmpCount - 1);
        //Debug.Log("sceneHead = "+sceneHead);

        //Debug.Log("path = " + path);
        //Debug.Log("replacePath = " + replacePath);
        int assetCount = path.IndexOf(replacePath);
        assetCount += replacePath.Length + 1;

        int tmpLength = nameCount - assetCount;
        if (tmpLength > 0) {
            string subString = path.Substring(assetCount, path.Length - assetCount);
            //Debug.Log("subString = " + subString);

            string[] result = subString.Split("/".ToCharArray());


            return sceneHead + "/" + result[0];
        } else {
            return sceneHead;
        }
    }

    // 改变标记
    private static void ChangeAssetMark(FileInfo tmpFile, string markStr, Dictionary<string, string> theWriter) {
        string fullPath = tmpFile.FullName;
        int assetCount = fullPath.IndexOf("Assets");
        string assetPath = fullPath.Substring(assetCount, fullPath.Length - assetCount);

        AssetImporter importer = AssetImporter.GetAtPath(assetPath);
        importer.assetBundleName = markStr;

        // 加后缀
        if (tmpFile.Extension == ".unity") {  // 场景文件
            importer.assetBundleVariant = "u3d";
        } else {
            importer.assetBundleVariant = "ld";
        }

        string modleName = "";
        string[] subMark = markStr.Split('/');
        if (subMark.Length > 1) {         // SceneOne/Load -- Load
            modleName = subMark[1];
        } else {                        // SceneOne      -- SceneOne
            modleName = markStr;
        }

        // sceneone/load.ld
        string modlePath = markStr.ToLower() + "." + importer.assetBundleVariant;

        if (!theWriter.ContainsKey(modleName)) {
            theWriter.Add(modleName, modlePath);
        }
    }
}

