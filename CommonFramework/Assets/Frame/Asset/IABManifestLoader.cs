using System.Collections;
using UnityEngine;

// ****************************************************************
// 功能：Manifest加载
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class IABManifestLoader {
    public AssetBundleManifest assetManifest;
    public AssetBundle manifestLoader;
    private static IABManifestLoader instance = null;
    private string manifestPaht;
    private bool isLoadFinish;

    public static IABManifestLoader Instance {
        get {
            if (instance==null) {
                instance = new IABManifestLoader();
            }

            return instance;
        }
    }

    public bool IsLoadFinish {
        get {
            return isLoadFinish;
        }
    }

    public IABManifestLoader() {
        assetManifest = null;
        manifestLoader = null;
        isLoadFinish = false;

        manifestPaht = IPathTools.GetAssetBundlePath();
    }

    /// <summary>
    /// 设置路径
    /// </summary>
    /// <param name="path"></param>
    public void SetManifestPath(string path) {
        manifestPaht = path;
    }

    public IEnumerator LoadManifest() {
        WWW manifestWWW = new WWW(manifestPaht);

        yield return manifestWWW;

        if (!string.IsNullOrEmpty(manifestWWW.error)) {
            // 报错
            Debug.LogWarning(manifestWWW.error);
        }else {
            // 加载成功
            if (manifestWWW.progress>=1.0f) {
                manifestLoader = manifestWWW.assetBundle;
                assetManifest = manifestLoader.LoadAsset("AssetBundles.manifest") as AssetBundleManifest;
                isLoadFinish = true;
            }
        }
    }

    public string[] GetDependencies(string bundleName) {
        return assetManifest.GetAllDependencies(bundleName);
    }

    public void UnloadManifest() {
        manifestLoader.Unload(true);
    }
}

