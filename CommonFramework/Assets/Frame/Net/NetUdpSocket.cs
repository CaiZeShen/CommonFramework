using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

// ****************************************************************
// 功能：底层通信
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class NetUdpSocket : MonoBehaviour {
    public delegate void UdpSocketDelegate(byte[] buffer, int dwCount, string ip, ushort port);

    private UdpSocketDelegate recvCallback;
    private IPEndPoint udpEP;
    private Socket udpSocket;
    private byte[] recvData;
    private Thread recvThread;
    private bool isRunning = true;

    public bool BindSocket(ushort port,int bufferLength, UdpSocketDelegate recvCallback) {
        udpEP = new IPEndPoint(IPAddress.Any, port);
        UdpConnent();
        this.recvCallback = recvCallback;
        recvData = new byte[bufferLength];

        if (this.recvCallback!=null) {
            recvThread = new Thread(RecvDataThread);
            recvThread.Start();
        }

        return true;
    }

    public int SendData(string ip,ushort port,byte[]data) {
        IPEndPoint sendToIP = new IPEndPoint(IPAddress.Parse(ip), port);

        if (udpSocket.Connected) {
            UdpConnent();
        }

        int mySend = udpSocket.SendTo(data, data.Length,SocketFlags.None,sendToIP);

        return mySend;
    }

    private void UdpConnent() {
        udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        udpSocket.Bind(udpEP);
    }

    private void RecvDataThread() {
        while (isRunning) {
            if (udpSocket==null || udpSocket.Available<1) {
                Thread.Sleep(100);
                continue;
            }

            lock (this) {
                // 创建一个空终端
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint remote = (EndPoint)sender;

                // 获取数据和发送端 
                int myCount = udpSocket.ReceiveFrom(recvData, ref remote);
                if (recvCallback!=null) {
                    recvCallback(recvData, myCount, remote.AddressFamily.ToString(), (ushort)sender.Port);
                }
            }
        }
    }
}

