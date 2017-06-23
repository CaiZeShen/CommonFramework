using UnityEngine;
using System;

// ****************************************************************
// 功能：单个包的加载资源
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class IABResLoader :IDisposable{
    private AssetBundle abRes;

    /// <summary>
    /// 加载单个资源
    /// </summary>
    /// <param name="resName"></param>
    /// <returns></returns>
    public UnityEngine.Object this[string resName] {
        get {
            if (abRes == null || !abRes.Contains(resName)) {
                Debug.LogWarning("res not contain " + resName);
                return null;
            }

            return abRes.LoadAsset(resName);
        }
    }

    public IABResLoader(AssetBundle bundle) {
        abRes = bundle;
    }

    /// <summary>
    /// 加载多个资源（一般用于切割成多个的精灵图）
    /// </summary>
    /// <param name="resName"></param>
    /// <returns></returns>
    public UnityEngine.Object[] LoadResources(string resName) {
        if (abRes == null || !abRes.Contains(resName)) {
            Debug.LogWarning("res not contain " + resName);
            return null;
        }

        return abRes.LoadAssetWithSubAssets(resName);
    }

    /// <summary>
    /// 卸载单个资源
    /// </summary>
    /// <param name="resObj"></param>
    public void UnLoadRes(UnityEngine.Object resObj) {
        Resources.UnloadAsset(resObj);
    }

    /// <summary>
    /// 释放 Assetbundle包
    /// </summary>
    public void Dispose() {
        if (abRes!=null) {
            abRes.Unload(false);
        }
    }

    /// <summary>
    /// 调试
    /// </summary>
    public void DebugAllRes() {
        string[] assetNames = abRes.GetAllAssetNames();
        for (int i = 0; i < assetNames.Length; i++) {
            Debug.Log("ABRes Contains AssetName = "+assetNames[i]);
        }
    }
}

