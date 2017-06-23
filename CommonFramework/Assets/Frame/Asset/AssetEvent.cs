using UnityEngine;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public enum AssetEvent {
    HunkRes=ManagerID.AssetManager+1,
    ReleaseSingleObj,
    ReleaseBundleObj,
    ReleaseSceneObj,

    ReleaseSingleBundle,
    ReleaseSceneBundle,
    ReleaseAll,
}

/// <summary>
/// 上层需要发给我们的消息参数
/// </summary>
public class HunkAssetRes : MsgBase {
    public string sceneName;
    public string bundleName;
    public string resName;
    public ushort backMagID;
    public bool isSingle;

    public HunkAssetRes(bool isSingle, ushort msgID, string sceneName, string bundleName, string resName, ushort backMagID) : base(msgID) {
        this.sceneName = sceneName;
        this.bundleName = bundleName;
        this.resName = resName;
        this.backMagID = backMagID;
        this.isSingle = isSingle;
    }
}

/// <summary>
/// 回上层的消息参数
/// </summary>
public class HunkAssetResBack : MsgBase {
    public Object[] value;

    public HunkAssetResBack() : base(0) {
        value = null;
    }

    public void Change(ushort msgid, params Object[] value) {
        this.msgID = msgid;
        this.value = value;
    }

    public void Change(ushort msgid) {
        this.msgID = msgid;
    }

    public void Change(params Object[] value) {
        this.value = value;
    }
}

