using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

// ****************************************************************
// 功能：底层socket tcp通信
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public enum ErrorSocket {
    Sucess = 0,
    TimeOut,
    SocketNull,
    SocketUnConnect,

    ConnectSucess,
    ConnectUnSucessUnKnown,
    ConnetcErrot,

    SendSucess,
    SendUnSucessUnKnown,
    ReciveUnSucessUnKnown,

    DisConnectSucess,
    DisConnectUnkown,
}

public class NetTcpSocket {
    public delegate void CallBackNormal(bool sucess, ErrorSocket error, string exception);

    public delegate void CallBackRecv(bool sucess, ErrorSocket error, string exception, byte[] byteMessage, string strMessag);

    private CallBackNormal callBackConnect;
    private CallBackNormal callBackSend;
    private CallBackNormal callBackDisconnect;
    private CallBackRecv callBackRecv;
    private ErrorSocket errorSocket;
    private Socket clientSocket;
    private string addressIP;
    private ushort port;
    private SocketBuffer recvSocketBuffer;
    private byte[] recvBuffer;

    public bool IsConnect {
        get {
            if (clientSocket!=null && clientSocket.Connected) {
                return true;
            }else {
                return false;
            }
        }
    }

    // 构造
    public NetTcpSocket() {
        recvSocketBuffer = new SocketBuffer(OnRecvMsgOver);
        recvBuffer = new byte[1024];
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    /// <param name="connectBack"></param>
    /// <param name="callBackRecv"></param>
    public void AsyncConnect(string ip, ushort port, CallBackNormal connectBack, CallBackRecv callBackRecv) {
        errorSocket = ErrorSocket.Sucess;

        this.callBackConnect = connectBack;
        this.callBackRecv = callBackRecv;


        if (clientSocket != null && clientSocket.Connected) {             // 已经连接
            this.callBackConnect(false, ErrorSocket.ConnetcErrot, "connect repeat!");
        } else if (clientSocket == null || !clientSocket.Connected) {     // 没有连接则开始连接
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            IAsyncResult connent = clientSocket.BeginConnect(endPoint, OnConnectCallback, clientSocket);
            if (!WriteDot(connent)) {
                this.callBackConnect(false, errorSocket, "连接超时！");
                clientSocket.EndConnect(connent);
            }
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="sendBuffer"></param>
    /// <param name="sendBack"></param>
    public void AsyncSend(byte[] sendBuffer, CallBackNormal sendBack) {
        errorSocket = ErrorSocket.Sucess;

        this.callBackSend = sendBack;

        if (clientSocket == null) {
            this.callBackSend(false, ErrorSocket.SocketNull, "");
        } else if (!clientSocket.Connected) {
            this.callBackSend(false, ErrorSocket.SocketUnConnect, "");
        } else {
            IAsyncResult asyncSend = clientSocket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, OnSendCallback, clientSocket);
            if (!WriteDot(asyncSend)) {
                this.callBackSend(false, ErrorSocket.SendUnSucessUnKnown, "发送超时");
            }
        }
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    public void AsyncRecive() {
        if (clientSocket != null && clientSocket.Connected) {             // 已经连接
            IAsyncResult asyncClient= clientSocket.BeginReceive(recvBuffer, 0, recvBuffer.Length, SocketFlags.None, OnReciveCallback, clientSocket);
            if (!WriteDot(asyncClient)) {
                callBackRecv(false, ErrorSocket.ReciveUnSucessUnKnown, "接收消息超时", null, "");
            }
            // TODO: 感觉老师写错，这里不用判断超时才对
        }
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    /// <param name="disconnectBack"></param>
    public void AsyncDisconnect(CallBackNormal disconnectBack) {
        try {
            errorSocket = ErrorSocket.Sucess;

            this.callBackDisconnect = disconnectBack;

            if (clientSocket == null) {
                this.callBackDisconnect(false, ErrorSocket.DisConnectUnkown, "client is null");
            } else if (!clientSocket.Connected) {
                this.callBackDisconnect(false, ErrorSocket.DisConnectUnkown, "client is unconnect");
            } else {
                IAsyncResult asyncDisConnect= clientSocket.BeginDisconnect(false, OnDisconnectCallback, clientSocket);

                if (!WriteDot(asyncDisConnect)) {
                    this.callBackDisconnect(false, ErrorSocket.DisConnectUnkown, "断开超时");
                }
            }

        } catch (Exception e) {
            this.callBackDisconnect(false, ErrorSocket.DisConnectUnkown, e.ToString());
        }
    }

    private void OnConnectCallback(IAsyncResult ar) {
        try {
            clientSocket.EndConnect(ar);

            if (!clientSocket.Connected) {      //连接失败
                errorSocket = ErrorSocket.ConnectUnSucessUnKnown;
                callBackConnect(false, errorSocket, "连接超时");
                return;
            } else {                            // 连接成功
                errorSocket = ErrorSocket.ConnectSucess;
                callBackConnect(true, errorSocket, "连接成功");

            }
        } catch (Exception e) {

            callBackConnect(false, errorSocket, e.ToString());
        }
    }

    private void OnSendCallback(IAsyncResult ar) {
        try {
            int sendLength = clientSocket.EndSend(ar);

            if (sendLength > 0) {
                callBackSend(true, ErrorSocket.SendSucess, "");
            } else {
                callBackSend(false, ErrorSocket.SendUnSucessUnKnown, "");
            }

        } catch (Exception e) {
            callBackSend(false, ErrorSocket.SendUnSucessUnKnown, e.ToString());
        }
    }

    private void OnReciveCallback(IAsyncResult ar) {
        try {
            if (!clientSocket.Connected) {      //没连接
                callBackRecv(false, ErrorSocket.ReciveUnSucessUnKnown, "连接失败", null, "");
                return;
            }

            int length = clientSocket.EndReceive(ar);
            if (length == 0) {
                return;
            }

            // 处理沾包
            recvSocketBuffer.RecvByte(recvBuffer, length);

        } catch (Exception e) {
            callBackRecv(false, ErrorSocket.ReciveUnSucessUnKnown, e.ToString(), null, "");
        }

        AsyncRecive();
    }

    private void OnDisconnectCallback(IAsyncResult ar) {
        try {
            clientSocket.EndDisconnect(ar);

            clientSocket.Close();
            clientSocket = null;

            callBackDisconnect(true, ErrorSocket.DisConnectSucess, "");
        } catch (Exception e) {
            callBackDisconnect(false, ErrorSocket.DisConnectUnkown, e.ToString());
        }
    }

    private void OnRecvMsgOver(byte[] allByte) {
        callBackRecv(true, ErrorSocket.Sucess, "", null, "recive back sucess");
    }

    // 超时检测
    private bool WriteDot(IAsyncResult ar) {
        ushort maxTime = 20;
        ushort i = 0;

        while (!ar.IsCompleted) {
            i++;
            if (i > maxTime) {
                errorSocket = ErrorSocket.TimeOut;
                return false;
            }

            Thread.Sleep(100);
        }

        return true;
    }
}

