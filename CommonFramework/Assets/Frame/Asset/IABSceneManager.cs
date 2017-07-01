using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Collections;

// ****************************************************************
// 功能：场景bundle包名字映射配置的管理
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class IABSceneManager {
    private Dictionary<string, string> allAssets;       // 保存bundle名字 -- bundle全名
    private IABManager abManager;
    private string sceneName;

    public IABSceneManager(string sceneName) {
        this.sceneName = sceneName;
        allAssets = new Dictionary<string, string>();
        abManager = new IABManager(sceneName);
    }

    /// <summary>
    /// 加载bundle
    /// </summary>
    /// <param name="bundleName"> Load </param>
    /// <param name="loadProgress"></param>
    /// <param name="callback"></param>
    public void LoadAsset(string bundleName,LoadProgress loadProgress,LoadAssetBundleCallBack callback) {
        if (allAssets.ContainsKey(bundleName)) {
            // sceneone/load.ld
            string name = allAssets[bundleName];

            abManager.LoadAssetBundle(name, loadProgress, callback);
        } else {
            Debug.LogWarning("Dont contains the bundle = " + bundleName);
        }
    }

    /// <summary>
    /// 读取配置
    /// </summary>
    /// <param name="sceneName">场景名字</param>
    public void ReadConfiger() {
        string path = IPathTools.GetAssetBundlePath() + "/" + sceneName+"/"+ "Record.txt";

        ReadConfig(path);
    }

    // 获取映射的名字
    public string GetBundleReflectName(string bundleName) {
        if (allAssets.ContainsKey(bundleName)) {
            return allAssets[bundleName];
        } else {
            return null;
        }
    }

    private void ReadConfig(string path) {
        FileStream fs = new FileStream(path, FileMode.Open);
        StreamReader br = new StreamReader(fs);

        // 读第一行(第一行写入的是总行数)
        int count = int.Parse(br.ReadLine());

        for (int i = 0; i < count; i++) {
            string tmpStr = br.ReadLine();
            string[] tmpArr= tmpStr.Split(' ');

            allAssets.Add(tmpArr[0], tmpArr[1]);
        }

        br.Close();
        fs.Close();
    }

    #region 由下层提供功能
    public IEnumerator LoadAssetSys(string bundleName) {
        yield return abManager.LoadAssetBundle(bundleName);
    } 

    /// <summary>
    /// 加载单个资源
    /// </summary>
    /// <param name="bundleName">包名</param>
    /// <param name="resName">资源名</param>
    /// <returns></returns>
    public Object GetSingleResources(string bundleName,string resName) {
        if (allAssets.ContainsKey(bundleName) ){
            return abManager.GetSingleResources(allAssets[bundleName], resName);
        } else {
            Debug.LogWarning("Dont contains the bundle = " + bundleName);
            return null;
        }
    }

    /// <summary>
    /// 加载多个资源（一般用于被切割多张的精灵图）
    /// </summary>
    /// <param name="bundleName">包名</param>
    /// <param name="resName">资源名</param>
    /// <returns></returns>
    public Object[] GetMultiResources(string bundleName,string resName) {
        if (allAssets.ContainsKey(bundleName)) {
            return abManager.GetMultiResources(allAssets[bundleName], resName);
        } else {
            Debug.LogWarning("Dont contains the bundle = " + bundleName);
            return null;
        }
    }

    /// <summary>
    /// 释放包中的单个资源
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="resName"></param>
    public void DisposeResObj(string bundleName,string resName) {
        if (allAssets.ContainsKey(bundleName)) {
            abManager.DisposeResObj(allAssets[bundleName], resName);
        } else {
            Debug.LogWarning("Dont contains the bundle = " + bundleName);
        }
    }

    /// <summary>
    /// 释放整个包的资源
    /// </summary>
    /// <param name="bundleName"></param>
    public void DisposeBundleRes(string bundleName) {
        if (allAssets.ContainsKey(bundleName)) {
            abManager.DisposeResObj(allAssets[bundleName]);
        } else {
            Debug.LogWarning("Dont contains the bundle = " + bundleName);
        }
    }

    /// <summary>
    /// 释放所有的包的资源
    /// </summary>
    public void DisposeAllRes() {
        abManager.DisposeAllObj();
    }

    /// <summary>
    /// 释放整个包
    /// </summary>
    /// <param name="bundleName"></param>
    public void DisposeBundle(string bundleName) {
        if (allAssets.ContainsKey(bundleName)) {
            abManager.DisposeBundle(allAssets[bundleName]);
        } else {
            Debug.LogWarning("Dont contains the bundle = " + bundleName);
        }
    }

    /// <summary>
    /// 释放整个包和资源
    /// </summary>
    /// <param name="bundleName"></param>
    public void DisposeBundleAndObjs(string bundleName) {
        if (allAssets.ContainsKey(bundleName)) {
            abManager.DisposeBundleAndObjs(allAssets[bundleName]);
        } else {
            Debug.LogWarning("Dont contains the bundle = " + bundleName);
        }
    }

    /// <summary>
    /// 释放所有包
    /// </summary>
    public void DisposeAllBundles() {
        abManager.DisposeAllBundle();

        allAssets.Clear();
    }

    /// <summary>
    /// 释放所有包和资源
    /// </summary>
    public void DisposeAllBundleAndRes() {
        abManager.DisposeAllBundleAndRes();

        allAssets.Clear();
    }

    public void DebugAllAsset() {
        foreach (string bundleName in allAssets.Values) {
            abManager.DebugAssetBundle(bundleName);
        }
    }

    public bool IsLoadingFinish(string bundleName) {
        if (allAssets.ContainsKey(bundleName)) {
            return abManager.IsLoadingFinish(allAssets[bundleName]);
        } else {
            Debug.Log("is not contains bundle = " + bundleName);
            return false;
        }
    }

    public bool IsLoadingAssetBundle(string bundleName) {
        if (allAssets.ContainsKey(bundleName)) {
            return abManager.IsLoadingAssetBundle(allAssets[bundleName]);
        } else {
            Debug.Log("is not contains bundle = " + bundleName);
            return false;
        }
    }
    #endregion
}

