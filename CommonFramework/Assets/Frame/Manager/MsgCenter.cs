using System;
using UnityEngine;

// ****************************************************************
// 功能：向各个manager转发消息
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class MsgCenter : MonoBase {
    public static MsgCenter Instance {
        get;
        private set;
    }

    public void SendToMsg(MsgBase msg) {

        AnalysisMsg(msg);
    }

    public override void ProcessEvent(MsgBase msg) {
        
    }

    private void Awake() {
        Instance = this;

        gameObject.AddComponent<UIManager>();
        gameObject.AddComponent<NPCManager>();
        // 其他模块
    }

    // 消息分析
    private void AnalysisMsg(MsgBase msg) {
        ManagerID managerID = msg.GetManagerID();
        switch (managerID) {
            case ManagerID.GameManager:
                break;
            case ManagerID.UIManager:
                UIManager.Instance.SendMsg(msg);
                break;
            case ManagerID.AudioManager:
                break;
            case ManagerID.NPCManager:
                NPCManager.Instance.SendMsg(msg);
                break;
            case ManagerID.CharactorManager:
                break;
            case ManagerID.AssetManager:
                break;
            case ManagerID.NetManger:
                break;
            case ManagerID.UIManagerTwo:
                break;
            default:
                break;
        }
    }
}

