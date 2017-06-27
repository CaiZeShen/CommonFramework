using System;
using UnityEngine;
using System.IO;

// ****************************************************************
// 功能：处理沾包问题
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class SocketBuffer {
    public delegate void CallBackRecvOver(byte[] allData);

    private const byte headLength = 6;      // 消息头长度,前四个字节表示消息的长度(一般不包含头部的长度)+两个字节msgid命令 
    private byte[] headByte;                // 消息头
    private byte[] allRecvData;             // 接收到的数据
    private int curRecvLength;              // 当前接收到的数据长度
    private int allDataLength;              // 总共接收的数据长度
    private CallBackRecvOver callBackRecvOver;

    // 构造
    public SocketBuffer(CallBackRecvOver recvOver) {
        headByte = new byte[headLength];
        this.callBackRecvOver = recvOver;
    }

    /// <summary>
    /// 接收数据
    /// </summary>
    /// <param name="recvByte"></param>
    /// <param name="length"></param>
    public void RecvByte(byte[] recvByte, int realLength) {
        if (realLength == 0) {
            return;
        }

        if (curRecvLength<headByte.Length) {  // 当前接收的数据小于头的长度
            RecvHead(recvByte, realLength);
        } else {
            // 接受的总长度
            int tmpLength = curRecvLength + realLength;
            if (tmpLength==allDataLength) {
                RecvOneALL(recvByte, realLength);
            } else if (tmpLength > allDataLength) {     // 接收的数据比这个消息长
                RecvLarger(recvByte, realLength);
            } else {
                RecvSmaller(recvByte, realLength);
            }
        }
    }

    // 处理头消息
    private void RecvHead(byte[] recvByte,int realLength) {
        // 差多少个字节才能组成一个头
        int tmpReal = headByte.Length - curRecvLength;

        // 接收到整个数据后的长度
        int tmpLength = curRecvLength + realLength;

        
        if (tmpLength<headByte.Length) {            // 总长度小于头部（接收到不完整的头部数据)
            // 把未完整的头部数据先存到headByte
            Buffer.BlockCopy(recvByte, 0, headByte, curRecvLength, realLength);

            curRecvLength += realLength;
        } else {                                    // 总长度大于等于头部
            // 凑齐头部
            Buffer.BlockCopy(recvByte, 0, headByte, curRecvLength, tmpReal);

            curRecvLength += tmpReal;

            // 取出四个字节转换为int（int32占4位字节）
            allDataLength = BitConverter.ToInt32(headByte, 0) + headLength;
            // body + head
            allRecvData = new byte[allDataLength];
            // allRecvData 已结包含了头部
            Buffer.BlockCopy(headByte, 0, allRecvData, 0, headLength);

            // 接收到的数据用于凑齐头部后是否还剩有数据
            int tmpRemin = realLength - tmpReal;
            if (tmpRemin>0) {
                byte[] tmpByte = new byte[tmpRemin];
                // 将剩下的字节拷贝到tmpByte
                Buffer.BlockCopy(recvByte, tmpReal, tmpByte, 0, tmpRemin);

                // 递归
                RecvByte(tmpByte, tmpRemin);
            } else {
                // 只有消息头的情况
                RecvOneMsgOver();
                // TODO:这里应该不做任何处理才对吧
            }
        }
    }

    // 接收完一条消息
    private void RecvOneMsgOver() {
        if (callBackRecvOver!=null) {
            callBackRecvOver(allRecvData);
        }
        curRecvLength = 0;
        allDataLength = 0;
        allRecvData = null;
    }

    private void RecvOneALL(byte[] recvByte, int realLength) {
        Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, realLength);
        curRecvLength += realLength;
        RecvOneMsgOver();
    }

    private void RecvLarger(byte[] recvByte, int realLength) {
        // 差多少的数据才完整
        int tmplength = allDataLength - curRecvLength;
        // 把差的数据拷贝到allRecvData
        Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, tmplength);
        curRecvLength += tmplength;

        RecvOneMsgOver();

        // 剩下的数据拷贝到reaminByte
        int remainLength = realLength - tmplength;
        byte[] reaminByte = new byte[remainLength];
        Buffer.BlockCopy(recvByte, tmplength, reaminByte, 0, remainLength);

        //（递归） 看成从 socket 里面取出来放入处理
        RecvByte(reaminByte,remainLength);
    }

    private void RecvSmaller(byte[] recvByte, int realLength) {
        Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, realLength);

        curRecvLength += realLength;
    }
}

