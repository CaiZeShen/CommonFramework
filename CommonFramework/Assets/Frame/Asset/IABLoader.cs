using UnityEngine;
using System.Collections;

public delegate void LoadProgress(string bundleName, float progress);
public delegate void LoadFinish(string bundleName);

// 单个包的加载
public class IABLoader {
    private string bundleName;
    private string commonBundlePath;
    private WWW commonLoader;
    private float commonResLoaderProcess;
    private LoadProgress loadProgress;
    private LoadFinish loadFinish;
    private IABResLoader abResLoader;

    public IABLoader(LoadProgress loadProgress, LoadFinish loadFinish) {
        bundleName = "";
        commonBundlePath = "";
        commonResLoaderProcess = 0;
        loadProgress = null;
        loadFinish = null;
        abResLoader = null;
        this.loadProgress = loadProgress;
        this.loadFinish = loadFinish;
    }

    /// <summary>
    /// 设置包名（sceneone/test.prefab）
    /// </summary>
    /// <param name="bundleName"></param>
    public void SetBundleName(string bundleName) {
        this.bundleName = bundleName;
    }

    /// <summary>
    /// 要求上层传递完整路径
    /// </summary>
    /// <param name="path"></param>
    public void LoadResources(string path) {
        commonBundlePath = path;
    }

    public IEnumerator CommonLoad() {
        commonLoader = new WWW(commonBundlePath);

        while (!commonLoader.isDone) {
            commonResLoaderProcess = commonLoader.progress;

            if (loadProgress!=null) {
                loadProgress(bundleName, commonResLoaderProcess);
            }

            yield return null;
            // 感觉这两句没必要(我直接改成 yield return null)
            //yield return commonLoader.progress;
            //commonResLoaderProcess = commonLoader.progress;
        }

        // 是否加载完
        if (commonResLoaderProcess>=1.0f) {

            if (loadFinish != null) {
                loadFinish(bundleName);
            }
        } else {
            Debug.LogError("Load Bundle Error = " + bundleName);
        }

        commonLoader = null;
    }

    #region 下层提供的功能
    /// <summary>
    /// 获取单个资源
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public UnityEngine.Object GetResources(string name) {
        if (abResLoader==null) {
            return null;
        }

        return abResLoader[name];
    } 

    /// <summary>
    /// 获取多个资源
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public UnityEngine.Object[] GetMultiRes(string name) {
        if (abResLoader == null) {
            return null;
        }

        return abResLoader.LoadResources(name);
    }

    /// <summary>
    /// 调试
    /// </summary>
    public void DebugLoader() {
        if (commonLoader != null) {
            abResLoader.DebugAllRes();
        }
    }

    /// <summary>
    /// 释放功能
    /// </summary>
    public void DisPose() {
        if (abResLoader != null) {
            abResLoader.Dispose();
            abResLoader = null;
        }
    }

    /// <summary>
    /// 卸载单个资源
    /// </summary>
    /// <param name="obj"></param>
    public void UnLoadAssetRes(UnityEngine.Object obj) {
        if (abResLoader != null) {
            abResLoader.UnLoadRes(obj);
        }
    }
    #endregion
}