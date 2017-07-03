using System;
using UnityEngine;

// ****************************************************************
// 功能：向UIManager注册mono还有一些msgID,一般是panel挂载这样的脚本,需要向其他模块或者脚本通信
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class UIBase : MonoBase {
    protected ushort[] msgIDs;

    public GameObject GetGameObject(string name) {
        return UIManager.Instance.GetGameObject(name);
    }

    public void RegistSelf(MonoBase mono,params ushort[] msgs) {
        UIManager.Instance.RegistMsg(mono, msgs);
    }

    public void UnRegistSelf(MonoBase mono, params ushort[] msgs) {
        UIManager.Instance.UnRegistMsg(mono, msgs);
    }

    public void SendMsg(MsgBase msg) {
        UIManager.Instance.SendMsg(msg);
    }

    public override void ProcessEvent(MsgBase msg) {
    }

    protected virtual void OnDestroy() {
        if (msgIDs!=null) {
            UnRegistSelf(this, msgIDs);
        }
    }
}

