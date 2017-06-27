using UnityEngine;
using System.Collections.Generic;
using System;

// ****************************************************************
// 功能：管理assetbundle模块加载
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class ILoaderManager:MonoBehaviour {
    // sceneName -- manager
    private Dictionary<string, IABSceneManager> sceneManagers = new Dictionary<string, IABSceneManager>();         
    private static ILoaderManager instance;

    public static ILoaderManager Instance {
        get {
            return instance;
        }
    }

    #region Unity 生命周期
    private void Awake() {
        instance = this;

        // 第一步 加载IABManifest
        StartCoroutine(IABManifestLoader.Instance.LoadManifest());

    }

    #endregion

    // 读取配置文件
    public void ReadConfiger(string sceneName) {
        if (!sceneManagers.ContainsKey(sceneName)) {
            IABSceneManager tmpManager = new IABSceneManager(sceneName);
            tmpManager.ReadConfiger();
            sceneManagers.Add(sceneName, tmpManager);
        }
    }

    // 提供加载功能
    public void LoadAsset(string sceneName, string bundleName, LoadProgress loadProgress) {
        if (!sceneManagers.ContainsKey(sceneName)) {
            ReadConfiger(sceneName);
        }

        // 加载指定包
        IABSceneManager sceneManager = sceneManagers[sceneName];
        sceneManager.LoadAsset(bundleName, loadProgress, OnLoadedCallBack);
    }

    // 回调 ，加载指定包完成，加载依赖包
    private void OnLoadedCallBack(string sceneName, string bundleName) {
        if (sceneManagers.ContainsKey(sceneName)) {
            IABSceneManager sceneManager = sceneManagers[sceneName];

            StartCoroutine(sceneManager.LoadAssetSys(bundleName));
        } else {
            Debug.Log("bundle name is not contains = " + bundleName);
        }
    }

    #region 由下层api提供
    /// <summary>
    /// 加载单个资源
    /// </summary>
    /// <param name="sceneName">SceneOne</param>
    /// <param name="bundleName">Load</param>
    /// <param name="resName">TestTwo</param>
    /// <returns></returns>
    public UnityEngine.Object GetSingleResources(string sceneName, string bundleName, string resName) {
        if (sceneManagers.ContainsKey(sceneName)) {
            IABSceneManager sceneMgr = sceneManagers[sceneName];

            return sceneMgr.GetSingleResources(bundleName, resName);
        } else {
            Debug.Log("sceneName = " + sceneName + " BundleName = " + bundleName + " is not load!");
            return null;
        }
    }

    /// <summary>
    /// 加载多个资源
    /// </summary>
    /// <param name="sceneName">SceneOne</param>
    /// <param name="bundleName">Load</param>
    /// <param name="resName">TestTwo</param>
    /// <returns></returns>
    public UnityEngine.Object[] GetMultiResources(string sceneName, string bundleName, string resName) {
        if (sceneManagers.ContainsKey(sceneName)) {
            IABSceneManager sceneMgr = sceneManagers[sceneName];

            return sceneMgr.GetMultiResources(bundleName, resName);
        } else {
            Debug.Log("sceneName = " + sceneName + " BundleName = " + bundleName + " is not load!");
            return null;
        }
    }

    /// <summary>
    /// 释放指定资源
    /// </summary>
    /// <param name="sceneName">SceneOne</param>
    /// <param name="bundleName">Load</param>
    /// <param name="resName">TestTwo</param>
    /// <returns></returns>
    public void UnLoadResObj(string sceneName,string bundleName ,string resName) {
        if (sceneManagers.ContainsKey(sceneName)) {
            IABSceneManager sceneMgr = sceneManagers[sceneName];

            sceneMgr.DisposeResObj(bundleName, resName);
        }
    }

    /// <summary>
    /// 释放指定包全部资源
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="bundleName"></param>
    public void UnLoadBundleResObjs(string sceneName, string bundleName) {
        if (sceneManagers.ContainsKey(sceneName)) {
            IABSceneManager sceneMgr = sceneManagers[sceneName];

            sceneMgr.DisposeBundleRes(bundleName);
        }
    }

    /// <summary>
    /// 释放所有包的资源
    /// </summary>
    /// <param name="sceneName"></param>
    public void UnLoadAllResObjs(string sceneName) {
        if (sceneManagers.ContainsKey(sceneName)) {
            IABSceneManager sceneMgr = sceneManagers[sceneName];

            sceneMgr.DisposeAllRes();
        }
    }

    /// <summary>
    /// 释放一个包
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="bundleName"></param>
    public void UnLoadAssetBundle(string sceneName,string bundleName) {
        if (sceneManagers.ContainsKey(sceneName)) {
            IABSceneManager sceneMgr = sceneManagers[sceneName];

            sceneMgr.DisposeBundle(bundleName);
        }
    }

    /// <summary>
    /// 释放指定场景所有包
    /// </summary>
    /// <param name="sceneName"></param>
    public void UnLoadAllAssetBundle(string sceneName) {
        if (sceneManagers.ContainsKey(sceneName)) {
            IABSceneManager sceneMgr = sceneManagers[sceneName];

            sceneMgr.DisposeAllBundles();

            System.GC.Collect();
        }
    }

    /// <summary>
    /// 释放指定场景所有包和资源
    /// </summary>
    /// <param name="sceneName"></param>
    public void UnLoadAllAssetBundleAndResObjs(string sceneName) {
        if (sceneManagers.ContainsKey(sceneName)) {
            IABSceneManager sceneMgr = sceneManagers[sceneName];

            sceneMgr.DisposeAllBundleAndRes();
        }
    }

    public void DebugAllAssetbundle(string sceneName) {
        if (sceneManagers.ContainsKey(sceneName)) {
            IABSceneManager sceneMgr = sceneManagers[sceneName];

            sceneMgr.DebugAllAsset();
        }
    }

    public bool IsLoadingBundleFinish(string sceneName, string bundleName) {
        if (sceneManagers.ContainsKey(sceneName)) {
            IABSceneManager tmpManager = sceneManagers[sceneName];

            return tmpManager.IsLoadingFinish(bundleName);
        }

        return false;
    }

    public bool IsLoadingAssetBundle(string sceneName, string bundleName) {
        if (sceneManagers.ContainsKey(sceneName)) {
            IABSceneManager tmpManager = sceneManagers[sceneName];

            return tmpManager.IsLoadingAssetBundle(bundleName);
        }

        return false;
    }

    // 获取映射的名字
    public string GetBundleReflectName(string sceneNmae,string bundleName) {
        IABSceneManager tmpManager = sceneManagers[sceneNmae];

        if (tmpManager!=null) {
            return tmpManager.GetBundleReflectName(bundleName);
        } else {
            return null;
        }
    }
    #endregion

    private void OnDestroy() {
        sceneManagers.Clear();
        System.GC.Collect();
    }
}

