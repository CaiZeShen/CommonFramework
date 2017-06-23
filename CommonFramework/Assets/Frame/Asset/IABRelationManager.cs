using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;

// ****************************************************************
// 功能：包的依赖关系的管理
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class IABRelationManager {
    private List<string> dependenceBundles;      // 依赖关系
    private List<string> referBundles;           // 被依赖关系
    private IABLoader assetLoader;
    private bool isLoadFinish;
    private string bundleName;
    private LoadProgress loadProgress;

    public string BundleName {
        get {
            return bundleName;
        }
    }

    public bool IsBundleLoadFinish {
        get { return isLoadFinish; }
    }

    public IABRelationManager() {
        dependenceBundles = new List<string>();
        referBundles = new List<string>();
    }

    public void Initialize(string bundleName, LoadProgress loadProgress) {
        this.loadProgress = loadProgress;
        this.bundleName = bundleName;
        isLoadFinish = false;

        assetLoader = new IABLoader(loadProgress, OnBundleLoadFinish);
        // 设置名字和路径
        assetLoader.SetBundleName(bundleName);
        string bundlePath = Path.Combine(IPathTools.GetWWWAssetBundlePath(), bundleName);
        assetLoader.LoadResources(bundlePath);
    }

    /// <summary>
    /// 添加ref关系
    /// </summary>
    /// <param name="bundleName"></param>
    public void AddReference(string bundleName) {
        referBundles.Add(bundleName);
    }

    /// <summary>
    /// 获取ref关系
    /// </summary>
    /// <returns></returns>
    public List<string> GetReference() {
        return referBundles;
    }

    /// <summary>
    ///  删除ref关系
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns>表示释放自己 </returns>
    public bool RemoveReference(string bundleName) {
        for (int i = 0; i < referBundles.Count; i++) {
            if (bundleName.Equals(referBundles[i])) {
                referBundles.RemoveAt(i);
            }
        }

        if (referBundles.Count<=0) {
            Dispose();
            return true;
        }

        return false;
    }

    /// <summary>
    /// 设置dependence关系
    /// </summary>
    /// <param name="dependence"></param>
    public void SetDependences(string[] dependence) {
        if (dependence.Length>0) {
            dependenceBundles.AddRange(dependence);
        }
    }

    /// <summary>
    /// 获取dependence关系
    /// </summary>
    /// <returns></returns>
    public List<string> GetDependences() {
        return dependenceBundles;
    }

    /// <summary>
    /// 删除dependence关系
    /// </summary>
    /// <param name="bundleName"></param>
    public void RemoveDependence(string bundleName) {
        for (int i = 0; i < dependenceBundles.Count; i++) {
            if (bundleName.Equals(dependenceBundles[i])) {
                dependenceBundles.RemoveAt(i);
            }
        }
    }

    public LoadProgress GetPorgress() {
        return loadProgress;
    }

    // 回调
    private void OnBundleLoadFinish(string bundleName) {
        isLoadFinish = true;
    }

    #region 由下层提供API
    // 释放
    public void Dispose() {
        assetLoader.DisPose();
    } 

    /// <summary>
    /// 获取单个资源
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public UnityEngine.Object GetSingleResource(string bundleName) {
        return assetLoader.GetResources(bundleName);
    }

    /// <summary>
    /// 获取多个资源
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public UnityEngine.Object[] GetMultiResources(string bundleName) {
        return assetLoader.GetMultiRes(bundleName);
    }

    public IEnumerator LoadAssetBundle() {
        yield return assetLoader.CommonLoad();
    }

    public void DebugAsset() {
        if (assetLoader!=null) {
            assetLoader.DebugLoader();
        } else {
            Debug.Log("asset load is null");
        }
    }
    #endregion
}

