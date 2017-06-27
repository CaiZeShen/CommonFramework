using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

// ****************************************************************
// 功能：消息处理层
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class NetTcpWorkToServer {
    private Queue<NetMsgBase> recvMsgPool = null;
    private Queue<NetMsgBase> sendMsgPool = null;
    private NetTcpSocket clientSocket;
    private Thread sendThread;
    
    // 构造
    public NetTcpWorkToServer(string ip, ushort port) {
        recvMsgPool = new Queue<NetMsgBase>();
        sendMsgPool = new Queue<NetMsgBase>();
        clientSocket = new NetTcpSocket();
        clientSocket.AsyncConnect(ip, port, OnConnectCallback, OnRecvCallback);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg"></param>
    public void PutSendMsgToPool(NetMsgBase msg) {
        lock (sendMsgPool) {

            sendMsgPool.Enqueue(msg);

        }
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    public void RecvUpdate() {
        while (recvMsgPool.Count > 0) {
            NetMsgBase tmpMsg = recvMsgPool.Dequeue();

            AnalyseData(tmpMsg);
        }
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public void Disconnect() {
        if (clientSocket!=null && clientSocket.IsConnect) {
            clientSocket.AsyncDisconnect(OnDisconnectCallback);
        }
    }

    private void OnDisconnectCallback(bool sucess, ErrorSocket error, string exception) {
        if (sucess) {
            // 关闭发送线程
            sendThread.Abort();
        } else {

        }
    }

    private void PuRecvMsgToPool(byte[] recvData) {
        NetMsgBase tmpMsg = new NetMsgBase(recvData);

        recvMsgPool.Enqueue(tmpMsg);
    }

    private void OnRecvCallback(bool sucess, ErrorSocket error, string exception, byte[] byteMessage, string strMessag) {
        if (sucess) {
            PuRecvMsgToPool(byteMessage);
        } else {
            // 处理错误消息
        }
    }

    private void OnConnectCallback(bool sucess, ErrorSocket error, string exception) {
        if (sucess) {
            sendThread = new Thread(LoopSendMsg);
            sendThread.Start();
        }
    }

    private void OnSendCallback(bool sucess, ErrorSocket error, string exception) {
        if (sucess) {

        } else {

        }
    }

    private void LoopSendMsg() {
        while (clientSocket != null && clientSocket.IsConnect) {

            lock (sendMsgPool) {

                while (sendMsgPool.Count > 0) {
                    NetMsgBase tmpSendMsg = sendMsgPool.Dequeue();

                    clientSocket.AsyncSend(tmpSendMsg.GetNetBytes(), OnSendCallback);
                }

            }

            Thread.Sleep(100);

        }
    }

    private void AnalyseData(NetMsgBase msg) {
        MsgCenter.Instance.SendToMsg(msg);
    }
}

