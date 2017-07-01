using UnityEngine;
using LuaInterface;
using System.Collections.Generic;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class MLuaResCallBackNode {
    public string resName;
    public string bundleName;
    public string sceneName;

    public bool isSingle;
    public MLuaResCallBackNode nextNode;
    public LuaFunction luaFunc;

    public MLuaResCallBackNode(string sceneName, string bundleName,string resName,LuaFunction luaFunc,bool isSingle,MLuaResCallBackNode nextNode) {
        this.resName = resName;
        this.bundleName = bundleName;
        this.sceneName = sceneName;
        this.luaFunc = luaFunc;
        this.isSingle = isSingle;
        this.nextNode = nextNode;
    }

    public void Dispose() {
        this.resName = null;
        this.bundleName = null;
        this.sceneName = null;
        this.luaFunc.Dispose();
        this.nextNode = null;
    }
}

public class MLuaResCallBackManager {
    Dictionary<string, MLuaResCallBackNode> nodes = null;

    public MLuaResCallBackManager() {
        nodes = new Dictionary<string, MLuaResCallBackNode>();
    }

    public void AddBundleCallBack(string bundleName, MLuaResCallBackNode node) {
        if (nodes.ContainsKey(bundleName)) {
            MLuaResCallBackNode topNode = nodes[bundleName];

            while (topNode.nextNode !=null) {
                topNode = topNode.nextNode;
            }
            topNode.nextNode = node;
        } else {
            nodes.Add(bundleName, node);
        }
    }

    public void Dispose(string bundleName) {
        if (nodes.ContainsKey(bundleName)) {
            MLuaResCallBackNode topNode = nodes[bundleName];

            while (topNode.nextNode != null) {
                MLuaResCallBackNode curNode = topNode;

                curNode.Dispose();

                topNode = topNode.nextNode;
            }
            topNode.Dispose();

            nodes.Remove(bundleName);
        }
    }

    public void CallBackLua(string bundleName) {
        if (nodes.ContainsKey(bundleName)) {
            MLuaResCallBackNode topNode = nodes[bundleName];

            do {

                if (topNode.isSingle) {
                    object tmpObj = ILoaderManager.Instance.GetSingleResources(topNode.sceneName, topNode.bundleName, topNode.resName);
                    topNode.luaFunc.Call(topNode.sceneName, topNode.bundleName, topNode.resName, tmpObj);
                } else {
                    object[] tmpObjs = ILoaderManager.Instance.GetMultiResources(topNode.sceneName, topNode.bundleName, topNode.resName);
                    topNode.luaFunc.Call(topNode.sceneName, topNode.bundleName, topNode.resName, tmpObjs);
                }

                topNode = topNode.nextNode;
            } while (topNode != null);
        }
    }
}

public class MLuaResLoader {
    private static MLuaResLoader instance = null;
    private MLuaResCallBackManager callBackManager = null;

    public static MLuaResLoader Instance {
        get {
            if (instance == null) {
                instance = new MLuaResLoader();
            }
            return instance;
        }
    }

    public MLuaResCallBackManager CallBackManager {
        get {
            if (callBackManager == null) {
                callBackManager = new MLuaResCallBackManager();
            }
            return callBackManager;
        }
    }

    public void GetResoures(string sceneName,string bundleName, string resName,bool isSingle,LuaFunction luaFunc) {
        if (!ILoaderManager.Instance.IsLoadingAssetBundle(sceneName, bundleName)) {                  // 没有加载
            // 加载并添加请求
            ILoaderManager.Instance.LoadAsset(sceneName, bundleName, OnLoadProgress);

            string bundleFullName = ILoaderManager.Instance.GetBundleReflectName(sceneName, bundleName);
            if (bundleFullName != null) {
                MLuaResCallBackNode tmpNode = new MLuaResCallBackNode(sceneName, bundleName, resName, luaFunc, isSingle,null);

                callBackManager.AddBundleCallBack(bundleFullName, tmpNode);
            } else {
                Debug.LogWarning("don't contains bundle = " + bundleName);
            }
        } else if (ILoaderManager.Instance.IsLoadingBundleFinish(sceneName, bundleName)) {          // 有加载，并且加载完成
            // 直接返回
            if (isSingle) {
                UnityEngine.Object tmpObj = ILoaderManager.Instance.GetSingleResources(sceneName, bundleName, resName);

                luaFunc.Call(sceneName, bundleName, resName, tmpObj);
            } else {
                UnityEngine.Object[] tmpObjs = ILoaderManager.Instance.GetMultiResources(sceneName, bundleName, resName);

                luaFunc.Call(sceneName, bundleName, resName, tmpObjs);
            }
        } else {                                                                                    // 已经加载，但没有完成
            // 把命令存下来

            string bundleFullName = ILoaderManager.Instance.GetBundleReflectName(sceneName, bundleName);
            if (bundleFullName != null) {
                MLuaResCallBackNode tmpNode = new MLuaResCallBackNode(sceneName, bundleName, resName, luaFunc, isSingle, null);

                callBackManager.AddBundleCallBack(bundleFullName, tmpNode);
            } else {
                Debug.LogWarning("don't contains bundle = " + bundleName);
            }
        }
    }

    /// <summary>
    /// 释放指定包中的指定资源
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="bundleName"></param>
    /// <param name="resName"></param>
    public void UnLoadResObj(string sceneName,string bundleName,string resName) {
        ILoaderManager.Instance.UnLoadResObj(sceneName, bundleName, resName);
    }

    /// <summary>
    /// 释放指定包中全部资源
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="bundleName"></param>
    public void UnLoadBundleObjs(string sceneName, string bundleName) {
        ILoaderManager.Instance.UnLoadBundleResObjs(sceneName, bundleName);
    }

    /// <summary>
    /// 释放指定包
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="bundleName"></param>
    public void UnLoadSingleBundle(string sceneName, string bundleName) {
        ILoaderManager.Instance.UnLoadAssetBundle(sceneName, bundleName);
    }

    /// <summary>
    /// 释放指定包和包中资源
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="bundleName"></param>
    public void UnLoadBundleAndObjs(string sceneName, string bundleName) {
        ILoaderManager.Instance.UnLoadAssetBundleAndObjs(sceneName, bundleName);
    }

    /// <summary>
    /// 释放所有包
    /// </summary>
    /// <param name="sceneName"></param>
    public void UnLoadAllBundle(string sceneName) {
        ILoaderManager.Instance.UnLoadAllAssetBundle(sceneName);
    }

    /// <summary>
    /// 释放所有包和资源
    /// </summary>
    /// <param name="sceneName"></param>
    public void UnLoadAllBundleAndObjs(string sceneName) {
        ILoaderManager.Instance.UnLoadAllAssetBundleAndResObjs(sceneName);
    }

    private void OnLoadProgress(string bundleName, float progress) {
        if (progress >= 1.0f) {

            callBackManager.CallBackLua(bundleName);
            callBackManager.Dispose(bundleName);
        }
    }

    
}

