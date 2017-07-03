using UnityEngine;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class MainOthers : UIBase {
    public override void ProcessEvent(MsgBase msg) {
        switch (msg.msgID) {
            case (ushort)UIEvent.CheckInitialFinish: {
                    gameObject.AddComponent<LuaClient>();
                }
                break;
        }
    }

    private void Awake() {
        msgIDs = new ushort[] {
            (ushort) UIEvent.CheckInitialFinish,
        };

        RegistSelf(this,msgIDs);

        LuaEventProcess.Instance.SettingChild(gameObject.AddComponent<LuaAndCMsgCenter>());

        gameObject.AddComponent<TcpSocket>();

        //gameObject.AddComponent<ExcelDataCenter>();

        //gameObject.AddComponent<GameLogic>();

        gameObject.AddComponent<NativeResLoader>();

        

    }

}

