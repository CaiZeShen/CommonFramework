using UnityEngine;

// ****************************************************************
// 功能：与框架结合
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class TcpSocket : NetBase {
    private NetTcpWorkToServer netWorkToServer = null;

    public override void ProcessEvent(MsgBase msg) {
        switch (msg.msgID) {
            case (ushort)TCPEvent.Connect: {
                    TCPConnectMsg connectMsg = (TCPConnectMsg)msg;
                    netWorkToServer = new NetTcpWorkToServer(connectMsg.ip, connectMsg.port);
                }
                break;
            case (ushort)TCPEvent.SendMsg: {
                    TCPSocketMsg sendMsg = (TCPSocketMsg)msg;
                    netWorkToServer.PutSendMsgToPool(sendMsg.netMsg);
                }
                break;
        }
    }

    #region Unity 生命周期
    private void Awake() {
        msgIDs = new ushort[] {
            (ushort)TCPEvent.Connect,
            (ushort)TCPEvent.SendMsg,

        };

        RegistSelf(this, msgIDs);
    }

    private void Update() {
        // 接收消息
        if (netWorkToServer != null) {
            netWorkToServer.RecvUpdate();
        }
    }
    #endregion
}

