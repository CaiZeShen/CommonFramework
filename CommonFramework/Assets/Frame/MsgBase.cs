using UnityEngine;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class MsgBase {
    // ushort 0~65535  两个字节
    public ushort msgID;

    public MsgBase() {
        msgID = 0;
    }

    public MsgBase(ushort msgID) {
        this.msgID = msgID;
    }

    public ManagerID GetManagerID() {
        int tempID = msgID / FrameTools.MsgSpan;
        return (ManagerID)(tempID * FrameTools.MsgSpan);
    }
}

