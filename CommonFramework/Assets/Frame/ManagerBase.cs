using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

// ****************************************************************
// 功能：消息的存储和处理
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class EventNode {
    public MonoBase data;           // 当前数据
    public EventNode next;          // 下一个节点

    public EventNode(MonoBase data) {
        this.data = data;
        this.next = null;
    }
}

public class ManagerBase : MonoBase {
    // 存储注册的消息
    private Dictionary<ushort, EventNode> eventTree = new Dictionary<ushort, EventNode>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mono">要注册的脚本</param>
    /// <param name="msgs">一个脚本可以注册多个msg</param>
    public void RegistMsg(MonoBase mono, params ushort[] msgs) {
        for (int i = 0; i < msgs.Length; i++) {
            EventNode node = new EventNode(mono);
            RegistMsg(msgs[i], node);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="msgID">消息id</param>
    /// <param name="node">node链表</param>
    public void RegistMsg(ushort msgID, EventNode node) {
        if (!eventTree.ContainsKey(msgID)) {
            //  数据链路里没有这个消息id
            eventTree.Add(msgID, node);
            return;
        }

        EventNode currentNode = eventTree[msgID];
        while (true) {
            // 防止重复添加一样数据的节点
            if (currentNode.data == node.data) {
                break;
            }

            if (currentNode.next == null) {
                currentNode.next = node;
                break;
            } else {
                currentNode = currentNode.next;
            }
        }
    }

    public void UnRegistMsg(MonoBase mono, params ushort[] msgs) {
        for (int i = 0; i < msgs.Length; i++) {
            UnRegistMsg(msgs[i], mono);
        }
    }

    public void UnRegistMsg(ushort msgID, MonoBase mono) {
        if (!eventTree.ContainsKey(msgID)) {
            return;
        }

        EventNode currentNode = eventTree[msgID];       // 当前节点指针
        EventNode preNode = null;                         // 上个节点指针
        while (true) {
            if (currentNode.data == mono) {// 找到数据,删除数据后退出
                if (preNode == null) {                // 1) 第一个节点
                    if (currentNode.next == null) {   // 最后一个节点,直接删掉字典的引用
                        eventTree.Remove(msgID);
                    } else {                        // 不是最后一个,把字典引用指向下一个
                        eventTree[msgID] = currentNode.next;
                    }
                } else {                            // 2) 不是第一个节点        
                    if (currentNode.next == null) { // 最后一个节点
                        preNode.next = null;
                    } else {                        // 不是最后一个
                        preNode.next = currentNode.next;
                        currentNode.next = null;
                    }
                }

                break;
            }

            // 到节点末尾,则退出
            if (currentNode.next == null)
                break;

            preNode = currentNode;
            currentNode = currentNode.next;
        }
    }

    // 测试用
    public void Show() {
        StringBuilder sb = new StringBuilder();
        foreach (ushort msgID in eventTree.Keys) {
            sb.Append(msgID.ToString() + ": ");
            EventNode currentNode = eventTree[msgID];
            do {
                sb.Append(currentNode.data + "->");

                currentNode = currentNode.next;
            } while (currentNode != null);
        }
        Debug.Log(eventTree.Count + " 条: " + sb.ToString());
    }

    // 来了消息,通知整个消息链表
    public override void ProcessEvent(MsgBase msg) {
        if (!eventTree.ContainsKey(msg.msgID)) {
            Debug.LogError("msg not contains msgID == " + msg.msgID);
            Debug.LogError("msg manager == " + msg.GetManagerID());
            return;
        }

        EventNode currentNode = eventTree[msg.msgID];
        do {
            currentNode.data.ProcessEvent(msg);

            currentNode = currentNode.next;
        } while (currentNode!=null);
    }
}