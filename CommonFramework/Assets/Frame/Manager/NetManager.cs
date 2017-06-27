using System.Collections.Generic;
using UnityEngine;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class NetManager : ManagerBase {
    private Dictionary<string, GameObject> sonMembers = new Dictionary<string, GameObject>();

    public static NetManager Instance {
        get;
        private set;
    }

    public GameObject GetGameObject(string name) {
        GameObject ret;
        sonMembers.TryGetValue(name, out ret);

        return ret;
    }

    public void RegistGameObject(string name, GameObject obj) {
        if (!sonMembers.ContainsKey(name)) {
            sonMembers.Add(name, obj);
        }
    }

    public void UnRegistGameObject(string name) {
        if (sonMembers.ContainsKey(name)) {
            sonMembers.Remove(name);
        }
    }

    public void SendMsg(MsgBase msg) {
        if (msg.GetManagerID() == ManagerID.NetManger) {
            // ManagerBase 本模块自己处理
            ProcessEvent(msg);
        } else {
            // MsgCenter 处理
            MsgCenter.Instance.SendToMsg(msg);
        }
    }

    private void Awake() {
        Instance = this;
    }
}

