using UnityEngine;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class AssetBase : MonoBase {
    protected ushort[] msgIDs;

    public void RegistSelf(MonoBase mono, params ushort[] msgs) {
        AssetManager.Instance.RegistMsg(mono, msgs);
    }

    public void UnRegistSelf(MonoBase mono, params ushort[] msgs) {
        AssetManager.Instance.UnRegistMsg(mono, msgs);
    }

    public void SendMsg(MsgBase msg) {
        AssetManager.Instance.SendMsg(msg);
    }

    public override void ProcessEvent(MsgBase msg) {
    }

    protected virtual void OnDestroy() {
        if (msgIDs != null) {
            UnRegistSelf(this, msgIDs);
        }
    }
}

