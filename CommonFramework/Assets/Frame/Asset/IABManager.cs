using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;

// ****************************************************************
// 功能：对一个场景的所有assetbundle包的管理
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

// 单个物体存取（有可能是多个物体组成，例如切割成多张的精灵图片）
public class AssetObj {
    public List<Object> objs;

    public AssetObj(params Object[] objs) {
        this.objs = new List<Object>();
        this.objs.AddRange(objs);
    }

    public void ReleaseObj() {
        for (int i = 0; i < objs.Count; i++) {
            Resources.UnloadAsset(objs[i]);
        }
    }
}

// 多个存取 （存的是一个bundle包的assetObj）
public class AssetResObj {
    private Dictionary<string, AssetObj> resObjs;

    public AssetResObj(string name, AssetObj obj) {
        resObjs = new Dictionary<string, AssetObj>();
        resObjs.Add(name, obj);
    }

    public void AddResObj(string name, AssetObj obj) {
        resObjs.Add(name, obj);
    }

    // 释放全部
    public void ReleaseAllResObj() {
        foreach (AssetObj assetObj in resObjs.Values) {
            assetObj.ReleaseObj();
        }
    }

    // 释放单个
    public void ReleaseResObj(string name) {
        if (resObjs.ContainsKey(name)) {
            resObjs[name].ReleaseObj();
        } else {
            Debug.Log("release object name is no exist = " + name);
        }
    }

    public List<Object> GetResObj(string name) {
        if (resObjs.ContainsKey(name)) {
            return resObjs[name].objs;
        } else {
            return null;
        }
    }
}

public delegate void LoadAssetBundleCallBack(string sceneName, string bundleName);

public class IABManager {
    private Dictionary<string, IABRelationManager> loadHelper;          // 把每个包都存起来
    private Dictionary<string, AssetResObj> loadObjs;                   // 把每个包的所有物体存起来
    private string sceneName;

    public IABManager(string sceneName) {
        loadHelper = new Dictionary<string, IABRelationManager>();
        loadObjs = new Dictionary<string, AssetResObj>();
        this.sceneName = sceneName;
    }

    /// <summary>
    /// 是否加载了bundle
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public bool IsLoadingAssetBundle(string bundleName) {
        return loadHelper.ContainsKey(bundleName);
    }

    /// <summary>
    /// 加载bundle
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="loadProgress"></param>
    /// <param name="callBack"></param>
    public void LoadAssetBundle(string bundleName, LoadProgress loadProgress, LoadAssetBundleCallBack callBack) {
        if (!loadHelper.ContainsKey(bundleName)) {
            IABRelationManager loader = new IABRelationManager();
            loader.Initialize(bundleName, loadProgress);

            loadHelper.Add(bundleName, loader);
            callBack(sceneName, bundleName);
        } else {
            Debug.Log("IABManager have contain bundle name = " + bundleName);
        }
    }

    // 加载assetbundle必须先加载manifest
    public IEnumerator LoadAssetBundle(string bundleName) {
        // 等待manifest加载完
        while (!IABManifestLoader.Instance.IsLoadFinish) {
            yield return null;
        }

        // 获取这个包的依赖关系
        IABRelationManager relationMgr = loadHelper[bundleName];
        string[] dependencies = GetDependencies(bundleName);
        relationMgr.SetDependences(dependencies);

        // 加载所有依赖的包
        for (int i = 0; i < dependencies.Length; i++) {
            yield return LoadAssetBundleDepedencies(dependencies[i], bundleName, relationMgr.GetPorgress());
        }

        // 加载包
        yield return relationMgr.LoadAssetBundle();
    }

    // 加载依赖关系的包 和 LoadAssetBundle 形成递归
    private IEnumerator LoadAssetBundleDepedencies(string bundleName, string refName, LoadProgress loadProgress) {
        if (!loadHelper.ContainsKey(bundleName)) {          // 不存在包
            // 创建依赖关系管理
            IABRelationManager relationMgr = new IABRelationManager();
            relationMgr.Initialize(bundleName, loadProgress);
            if (refName != null) {
                relationMgr.AddReference(refName);
            }

            // 保存
            loadHelper.Add(bundleName, relationMgr);

            // 加载包
            yield return LoadAssetBundle(bundleName);
        } else {                                            // 存在包

            // 添加依赖关系
            if (refName != null) {
                IABRelationManager loader = loadHelper[bundleName];
                loader.AddReference(refName);
            }
        }
    }

    private string[] GetDependencies(string bundleName) {
        return IABManifestLoader.Instance.GetDependencies(bundleName);
    }

    #region 释放包资源
    /// <summary>
    /// 释放包中的指导资源
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="resName"></param>
    public void DisposeResObj(string bundleName, string resName) {
        if (loadObjs.ContainsKey(bundleName)) {
            AssetResObj tmpObj = loadObjs[bundleName];
            tmpObj.ReleaseResObj(resName);
        }
    }

    /// <summary>
    /// 释放整个包的资源
    /// </summary>
    /// <param name="bundleName"></param>
    public void DisposeResObj(string bundleName) {
        if (loadObjs.ContainsKey(bundleName)) {
            AssetResObj tmpObj = loadObjs[bundleName];
            tmpObj.ReleaseAllResObj();
        }

        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// 释放所有的包的资源
    /// </summary>
    public void DisposeAllObj() {
        foreach (string bundleName in loadObjs.Keys) {
            DisposeResObj(bundleName);
        }

        loadObjs.Clear();
    }

    /// <summary>
    /// 删除单个包并递归处理包的依赖关系
    /// </summary>
    /// <param name="bundleName"></param>
    public void DisposeBundle(string bundleName) {
        if (loadHelper.ContainsKey(bundleName)) {
            // 遍历包的所有依赖包
            IABRelationManager loader = loadHelper[bundleName];
            List<string> dependencies = loader.GetDependences();
            for (int i = 0; i < dependencies.Count; i++) {

                if (loadHelper.ContainsKey(dependencies[i])) {  // 当前依赖包存在内存
                    // 删除和当前包的被依赖关系
                    IABRelationManager currentRelationMgr = loadHelper[dependencies[i]];
                    if (currentRelationMgr.RemoveReference(bundleName)) {   // 当前依赖包的被依赖关系为空（函数内部删除当前依赖包）
                        // 删除当前依赖包的的依赖包（递归）
                        DisposeBundle(currentRelationMgr.BundleName);
                    }
                }
            }

            // 当前包的被依赖关系为空时，删除当前包
            if (loader.GetReference().Count <= 0) {
                loader.Dispose();
                loadHelper.Remove(bundleName);
            }
        }
    }

    /// <summary>
    /// 删除所有的包
    /// </summary>
    public void DisposeAllBundle() {

        List<string> keys = new List<string>();

        keys.AddRange(loadHelper.Keys);

        for (int i = 0; i < loadHelper.Count; i++) {
            IABRelationManager loader = loadHelper[keys[i]];
            loader.Dispose();
        }

        loadHelper.Clear();
    }

    /// <summary>
    /// 删除所有的包和资源
    /// </summary>
    public void DisposeAllBundleAndRes() {
        DisposeAllObj();

        DisposeAllBundle();
    }

    #endregion

    #region 由下层提供api
    /// <summary>
    /// 调试
    /// </summary>
    /// <param name="bundleName">sceneone/test.prefab</param>
    public void DebugAssetBundle(string bundleName) {
        if (loadHelper.ContainsKey(bundleName)) {
            loadHelper[bundleName].DebugAsset();
        }
    }

    public bool IsLoadingFinish(string bundleName) {
        if (loadHelper.ContainsKey(bundleName)) {
            return loadHelper[bundleName].IsBundleLoadFinish;
        } else {
            Debug.Log("IABRelation no cantains bundle = " + bundleName);
            return false;
        }
    }

    /// <summary>
    /// 加载的单个资源
    /// </summary>
    /// <param name="bundleName">包名</param>
    /// <param name="resName">资源名</param>
    /// <returns></returns>
    public Object GetSingleResources(string bundleName, string resName) {
        // 是否已经缓存了这个包
        if (loadObjs.ContainsKey(bundleName)) {
            // 缓存包中有指定资源，直接返回
            AssetResObj tmpRes = loadObjs[bundleName];
            List<Object> tmpObjs = tmpRes.GetResObj(resName);

            if (tmpObjs != null) {
                return tmpObjs[0];
            }
        }

        // 是否已经加载过bundle
        if (loadHelper.ContainsKey(bundleName)) {
            // 从包中加载出obj
            IABRelationManager loader = loadHelper[bundleName];
            Object tmpObj = loader.GetSingleResource(resName);
            AssetObj tmpAssetObj = new AssetObj(tmpObj);

            // 是否已经缓存了这个包
            if (loadObjs.ContainsKey(bundleName)) {
                // 添加缓存资源
                AssetResObj tmpRes = loadObjs[bundleName];
                tmpRes.AddResObj(resName, tmpAssetObj);
            } else {
                // 添加缓存包和资源
                AssetResObj tmpRes = new AssetResObj(resName, tmpAssetObj);
                loadObjs.Add(bundleName, tmpRes);
            }

            return tmpObj;
        } else {
            return null;
        }
    }

    /// <summary>
    /// 加载多个资源（一般用于切割多张的精灵图片）
    /// </summary>
    /// <param name="bundleName">包名</param>
    /// <param name="resName">资源名</param>
    /// <returns></returns>
    public Object[] GetMultiResources(string bundleName, string resName) {
        // 是否已经缓存了这个包
        if (loadObjs.ContainsKey(bundleName)) {
            // 缓存包中有指定资源，直接返回
            AssetResObj tmpRes = loadObjs[bundleName];
            List<Object> tmpObjs = tmpRes.GetResObj(resName);

            if (tmpObjs != null) {
                return tmpObjs.ToArray();
            }
        }

        // 是否已经加载过bundle
        if (loadHelper.ContainsKey(bundleName)) {
            // 从包中加载出obj
            IABRelationManager loader = loadHelper[bundleName];
            Object[] tmpObjs = loader.GetMultiResources(resName);
            AssetObj tmpAssetObj = new AssetObj(tmpObjs);

            // 是否已经缓存了这个包
            if (loadObjs.ContainsKey(bundleName)) {
                // 添加缓存资源
                AssetResObj tmpRes = loadObjs[bundleName];
                tmpRes.AddResObj(resName, tmpAssetObj);
            } else {
                // 添加缓存包和资源
                AssetResObj tmpRes = new AssetResObj(resName, tmpAssetObj);
                loadObjs.Add(bundleName, tmpRes);
            }

            return tmpObjs;
        } else {
            return null;
        }
    }
    #endregion
}

