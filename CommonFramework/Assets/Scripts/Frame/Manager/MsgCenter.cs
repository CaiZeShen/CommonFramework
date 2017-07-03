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

        gameObject.AddComponent<LuaEventProcess>();
        // 其他模块
    }

    // 消息分析
    private void AnalysisMsg(MsgBase msg) {
        if (msg == null) {
            return;
        }

        int tmpId = msg.GetMananger();

        if (tmpId < (int)ManagerID.NetManager) {        //  lua的消息
            LuaEventProcess.Instance.ProcessEvent(msg);
        } else {                                        // c#的消息

            ManagerID managerID = (ManagerID)tmpId;
            switch (managerID) {
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
                case ManagerID.AssetMananger:
                    break;
                case ManagerID.NetManager:
                    break;
                case ManagerID.DataManager:
                    break;
                default:
                    break;
            }

        }
        
    }
}

