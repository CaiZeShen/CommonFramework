using UnityEngine;
using System.Collections.Generic;
using System;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public delegate void NatvieResCallBack(NativeResCallBackNode node);

public class NativeResCallBackNode {
    public string sceneName;
    public string bundleName;
    public string resName;
    public ushort backMagID;
    public bool isSingle;
    public NativeResCallBackNode nextNode;
    public NatvieResCallBack callBack;

    public NativeResCallBackNode(bool isSingle, string sceneName, string bundleName, string resName, ushort backMagID, NatvieResCallBack callBack,NativeResCallBackNode nextNode) {
        this.sceneName = sceneName;
        this.bundleName = bundleName;
        this.resName = resName;
        this.backMagID = backMagID;
        this.isSingle = isSingle;
        this.nextNode = nextNode;
        this.callBack = callBack;
    }

    public void Dispose() {
        nextNode = null;
        callBack = null;
        sceneName = null;
        bundleName = null;
        resName = null;
    }
}

public class NatvieResCallBackManager {
    private Dictionary<string, NativeResCallBackNode> callBackNodes;

    public NatvieResCallBackManager() {
        callBackNodes = new Dictionary<string, NativeResCallBackNode>();
    }

    /// <summary>
    /// 来了请求添加过程
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="node"></param>
    public void AddBundle(string bundleName, NativeResCallBackNode node) {
        if (callBackNodes.ContainsKey(bundleName)) {
            NativeResCallBackNode tmpNode = callBackNodes[bundleName];

            // 存到链表最后
            while (tmpNode.nextNode != null) {
                tmpNode = tmpNode.nextNode;
            }
            tmpNode.nextNode = node;
        } else {
            callBackNodes.Add(bundleName, node);
        }
    }

    /// <summary>
    /// 加载完成后，消息向上层传递完成了，就把这些缓存的命令删除
    /// </summary>
    /// <param name="bundleName"></param>
    public void DisPose(string bundleName) {
        if (callBackNodes.ContainsKey(bundleName)) {
            NativeResCallBackNode curNode = callBackNodes[bundleName];
            NativeResCallBackNode nextNode=null;

            // 挨个释放node
            do {
                nextNode = curNode.nextNode;
                curNode.Dispose();

                curNode = nextNode;
            } while (curNode != null);

            callBackNodes.Remove(bundleName);
        }
    }

    public void CallBackRes(string bundleName) {
        if (callBackNodes.ContainsKey(bundleName)) {
            NativeResCallBackNode curNode = callBackNodes[bundleName];

            do {
                curNode.callBack(curNode);

                curNode = curNode.nextNode;
            } while (curNode != null);

        }
    }
}

public class NativeResLoader : AssetBase {
    private HunkAssetResBack resBackMsg = null;
    private NatvieResCallBackManager callBackMgr = null;

    public HunkAssetResBack ResBackMsg {
        get {
            if (resBackMsg == null) {
                resBackMsg = new HunkAssetResBack();
            }
            return resBackMsg;
        }
    }
    public NatvieResCallBackManager CallBackMgr {
        get {
            if (callBackMgr==null) {
                callBackMgr = new NatvieResCallBackManager();
            }
            return callBackMgr;
        }
    }

    public override void ProcessEvent(MsgBase msg) {

        switch (msg.msgID) {
            case (ushort)AssetEvent.ReleaseSingleObj: {
                    HunkAssetRes tmpMsg = msg as HunkAssetRes;
                    ILoaderManager.Instance.UnLoadResObj(tmpMsg.sceneName, tmpMsg.bundleName, tmpMsg.resName);
                }
                break;
            case (ushort)AssetEvent.ReleaseBundleObj: {
                    HunkAssetRes tmpMsg = msg as HunkAssetRes;
                    ILoaderManager.Instance.UnLoadBundleResObjs(tmpMsg.sceneName, tmpMsg.bundleName);
                }
                break;
            case (ushort)AssetEvent.ReleaseSceneObj: {
                    HunkAssetRes tmpMsg = msg as HunkAssetRes;
                    ILoaderManager.Instance.UnLoadAllResObjs(tmpMsg.sceneName);
                }
                break;
            case (ushort)AssetEvent.ReleaseSingleBundle: {
                    HunkAssetRes tmpMsg = msg as HunkAssetRes;
                    ILoaderManager.Instance.UnLoadAssetBundle(tmpMsg.sceneName, tmpMsg.bundleName);
                }
                break;
            case (ushort)AssetEvent.ReleaseSceneBundle: {
                    HunkAssetRes tmpMsg = msg as HunkAssetRes;
                    ILoaderManager.Instance.UnLoadAllAssetBundle(tmpMsg.sceneName);
                }
                break;
            case (ushort)AssetEvent.ReleaseAll: {
                    HunkAssetRes tmpMsg = msg as HunkAssetRes;
                    ILoaderManager.Instance.UnLoadAllAssetBundleAndResObjs(tmpMsg.sceneName);
                }
                break;
            case (ushort)AssetEvent.HunkRes: {      // 请求资源
                    HunkAssetRes tmpMsg = msg as HunkAssetRes;

                    GetResources(tmpMsg.sceneName, tmpMsg.bundleName, tmpMsg.resName, tmpMsg.isSingle, tmpMsg.backMagID);
                    //ILoaderManager.Instance.UnLoadAllAssetBundleAndResObjs(tmpMsg.sceneName);
                }
                break;
        }
    }

    private void GetResources(string sceneName, string bundleName, string resName, bool isSingle, ushort backID) {

        if (!ILoaderManager.Instance.IsLoadingAssetBundle(sceneName, bundleName)) {                  // 没有加载
            // 加载并添加请求
            ILoaderManager.Instance.LoadAsset(sceneName, bundleName, OnLoadProgress);

            string bundleFullName = ILoaderManager.Instance.GetBundleReflectName(sceneName, bundleName);
            if (bundleFullName != null) {
                NativeResCallBackNode tmpNode = new NativeResCallBackNode(isSingle, sceneName, bundleName, resName, backID, OnSendToBackMsg, null);

                CallBackMgr.AddBundle(bundleFullName, tmpNode);
            } else {
                Debug.LogWarning("don't contains bundle = " + bundleName);
            }
        } else if (ILoaderManager.Instance.IsLoadingBundleFinish(sceneName, bundleName)) {          // 有加载，并且加载完成
            // 直接返回
            if (isSingle) {
                UnityEngine.Object tmpObj = ILoaderManager.Instance.GetSingleResources(sceneName, bundleName, resName);

                ResBackMsg.Change(backID, tmpObj);
                SendMsg(ResBackMsg);
            } else {
                UnityEngine.Object[] tmpObjs = ILoaderManager.Instance.GetMultiResources(sceneName, bundleName, resName);

                ResBackMsg.Change(backID, tmpObjs);
                SendMsg(ResBackMsg);
            }
        } else {                                                                                    // 已经加载，但没有完成
            // 把命令存下来

            string bundleFullName = ILoaderManager.Instance.GetBundleReflectName(sceneName, bundleName);
            if (bundleFullName != null) {
                NativeResCallBackNode tmpNode = new NativeResCallBackNode(isSingle, sceneName, bundleName, resName, backID, OnSendToBackMsg, null);

                CallBackMgr.AddBundle(bundleFullName, tmpNode);
            } else {
                Debug.LogWarning("don't contains bundle = " + bundleName);
            }
        }
    }

    /// <summary>
    /// node 回调 回复上层消息
    /// </summary>
    /// <param name="node"></param>
    private void OnSendToBackMsg(NativeResCallBackNode node) {
        if (node.isSingle) {
            // 获取资源
            UnityEngine.Object tmpObj = ILoaderManager.Instance.GetSingleResources(node.sceneName, node.bundleName, node.resName);

            // 设置参数并发送
            ResBackMsg.Change(node.backMagID, tmpObj);
            SendMsg(ResBackMsg);
        } else {
            UnityEngine.Object[] tmpObj = ILoaderManager.Instance.GetMultiResources(node.sceneName, node.bundleName, node.resName);

            ResBackMsg.Change(node.backMagID, tmpObj);
            SendMsg(ResBackMsg);
        }
    }

    private void OnLoadProgress(string bundleName, float progress) {
        if (progress>=1.0f) {

            CallBackMgr.CallBackRes(bundleName);
            CallBackMgr.DisPose(bundleName);
        }
    }

    private void Awake() {
        msgIDs = new ushort[] {
            (ushort)AssetEvent.ReleaseSingleObj,
            (ushort)AssetEvent.ReleaseBundleObj,
            (ushort)AssetEvent.ReleaseSceneObj,

            (ushort)AssetEvent.ReleaseSingleBundle,
            (ushort)AssetEvent.ReleaseSceneBundle,
            (ushort)AssetEvent.ReleaseAll,

            (ushort)AssetEvent.HunkRes,
        };

        RegistSelf(this, msgIDs);
    }
}

