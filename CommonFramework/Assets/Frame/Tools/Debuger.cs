using UnityEngine;
using System.Text;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class Debuger {
    public static bool enableLog = true;
    public static NetUdpSocket netUdpSocket = null;

    public static NetUdpSocket NetUdpSocket {
        get {
            if (netUdpSocket==null) {
                netUdpSocket = new NetUdpSocket();
                netUdpSocket.BindSocket(18001, 1024, null);
            }
            return netUdpSocket;
        }
    }

    public static void Log(object message,Object context) {
        if (!enableLog) {
            return;
        }

        if (Application.platform==RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.OSXEditor) {
            Debug.Log(message, context);
        } else {
            byte[] data = Encoding.Default.GetBytes(message.ToString());
            NetUdpSocket.SendData("255.255.255.255",18001, data); // 填255.255.255.255 就可广播
        }
    }
}

