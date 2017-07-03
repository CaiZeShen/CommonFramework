using UnityEngine;
using System;
//using ProtoBuf;
// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

// 发送的时候 4 + 2 + proto     接收的时候 4 + 2 + 1 + proto
public class NetMsgBase : MsgBase {
    protected byte[] buffer;

    public NetMsgBase() {

    }

    public NetMsgBase(byte[] buffer) {
        this.buffer = buffer;

        // buffer 一般为6个字节，前四个字节表示消息的长度(一般不包含头部的长度)+两个字节msgid命令
        this.msgID = BitConverter.ToUInt16(this.buffer, 4);
    }

    // for lua send
    public NetMsgBase(ushort tmpMsgID,byte[] arr) {
        this.msgID = tmpMsgID;
        buffer = new byte[arr.Length + 6];
        Buffer.BlockCopy(arr, 0, buffer, 6, arr.Length);

        byte[] tmpLenght = BitConverter.GetBytes(arr.Length);
        Buffer.BlockCopy(tmpLenght, 0, buffer, 0, 4);

        byte[] tmpId = BitConverter.GetBytes(tmpMsgID);
        Buffer.BlockCopy(tmpId, 0, buffer, 4, 2);

    }

    public override byte[] GetNetBytes() {
        return buffer;
    }

    public override byte GetState() {
        return buffer[6];
    }

    public virtual byte[] GetProtoBuffer() {
        byte[] tmpByte = new byte[buffer.Length - 7];
        Buffer.BlockCopy(buffer, 7, tmpByte, 0, buffer.Length - 7);
        return tmpByte;
    }
}

public enum TCPEvent {
    Connect = ManagerID.NetManager + 1,   // 连接请求
    SendMsg,

    MaxValue,
}

public class TCPConnectMsg : MsgBase {
    public string ip;
    public ushort port;

    public TCPConnectMsg(ushort msgID, string ip, ushort port) {
        this.msgID = msgID;
        this.ip = ip;
        this.port = port;
    }
}

public class TCPSocketMsg : MsgBase {
    public NetMsgBase netMsg;

    public TCPSocketMsg() {
        netMsg = null;
        msgID = 0;
    }

    public TCPSocketMsg(ushort msgID, NetMsgBase netMsg) {
        this.msgID = msgID;
        this.netMsg = netMsg;
    }

    public void ChangerMsg(ushort msgID,NetMsgBase netMsg) {
        this.netMsg = netMsg;
        this.msgID = msgID;
    }

    public void ChangeLuaMsg(ushort msgID,LuaByteBuffer luaByte, ushort netID) {
        this.msgID = msgID;

        netMsg = new NetMsgBase(netID, luaByte.buffer);
    }
}

/// <summary>
/// protoBuffer c#专用
/// </summary>
/// <typeparam name="T"></typeparam>
//public class TcpNetMsgs<T> : NetMsgBase where T : IExtensible {

//    // 构造
//    public TcpNetMsgs(T tmp, ushort msgID) {
//        ChangeMsgData<T>(tmp, msgID);
        
//    }

//    /// <summary>
//    /// 从socket接收过来的数据转变成protobuffer类
//    /// </summary>
//    /// <typeparam name="U"></typeparam>
//    /// <returns></returns>
//    public U GetPBClass<U>() where U : IExtensible {
//        // 拷贝出body (-6是去掉头信息)
//        byte[] tmpByte = new byte[this.buffer.Length-6];
//        Buffer.BlockCopy(buffer, 6, tmpByte, 0, buffer.Length - 6);

//        return IProtoTools.Deserizlize<U>(tmpByte);
//    }

//    public U GetPBClass<U>(byte[] tmpArr) where U : IExtensible {
//        this.buffer = tmpArr;
//        return GetPBClass<U>();
//    }

//    public override byte[] GetNetBytes() {
//        return buffer;
//    }

//    public void ChangeMsgData<V>(V tmp)where V: IExtensible {
//        // 组成发送byte[]
//        byte[] tmpBytes = IProtoTools.Serialize(tmp);
//        buffer = new byte[tmpBytes.Length + 6];

//        // 消息长度
//        byte[] datalength = BitConverter.GetBytes(tmpBytes.Length);
//        Buffer.BlockCopy(datalength, 0, buffer, 0, 4);

//        // body
//        Buffer.BlockCopy(tmpBytes, 0, buffer, 6, tmpBytes.Length);
//    }

//    public void ChangeMsgData<V>(V tmp, ushort msgId) where V : IExtensible {
//        ChangeMsgData(msgId);
//        ChangeMsgData(tmp);
//    }

//    public void ChangeMsgData(ushort msgId) {
//        this.msgID = msgId;

//        // msgID
//        byte[] eventID = BitConverter.GetBytes(this.msgID);
//        Buffer.BlockCopy(eventID, 0, buffer, 4, 2);
//    }

//}

public enum UDPEvent {
    Initial = TCPEvent.MaxValue + 1,
    SendTo,
    maxValue,
}

public class UDPMsg : MsgBase {
    public ushort port;
    public int recvBufferLength;
    public NetUdpSocket.UdpSocketDelegate recvDelegate;

    public UDPMsg(ushort msgid,ushort port,int recvLength, NetUdpSocket.UdpSocketDelegate recvDelegate) {
        this.msgID = msgid;
        this.port = port;
        this.recvBufferLength = recvLength;
        this.recvDelegate = recvDelegate;
    }
}

public class UDPSendMsg : MsgBase {
    public string ip;
    public ushort port;
    public byte[] data;

    public UDPSendMsg(string ip, ushort port, byte[] data) {
        this.ip = ip;
        this.port = port;
        this.data = data;
    }
}