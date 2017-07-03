using UnityEngine;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class UdpSocket : NetBase {
    private NetUdpSocket netUdpSocket;

    public override void ProcessEvent(MsgBase msg) {
        switch (msg.msgID) {
            case (ushort)UDPEvent.Initial: {
                    UDPMsg tmpMsg = (UDPMsg)msg;

                    netUdpSocket = new NetUdpSocket();
                    netUdpSocket.BindSocket(tmpMsg.port, tmpMsg.recvBufferLength, tmpMsg.recvDelegate);
                }
                break;
            case (ushort)UDPEvent.SendTo: {
                    UDPSendMsg tmpSendMsg = (UDPSendMsg)msg;

                    netUdpSocket.SendData(tmpSendMsg.ip, tmpSendMsg.port, tmpSendMsg.data);
                }
                break;
        }
    }

    #region Unity 生命周期
    private void Awake() {
        msgIDs = new ushort[] {
            (ushort)UDPEvent.Initial,
            (ushort)UDPEvent.SendTo,
        };

        RegistSelf(this, msgIDs);
    } 
    #endregion
}

