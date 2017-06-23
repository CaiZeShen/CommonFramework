using UnityEngine;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class FrameTools {

    public const int MsgSpan = 3000;               // 每个基站能管理的消息量

    
}

public enum ManagerID {
    GameManager = 0,
    UIManager = FrameTools.MsgSpan,                 // 3000
    AudioManager = FrameTools.MsgSpan * 2,          // 6000
    NPCManager = FrameTools.MsgSpan * 3,
    CharactorManager = FrameTools.MsgSpan * 4,
    AssetManager = FrameTools.MsgSpan * 5,
    NetManger = FrameTools.MsgSpan * 6,
    UIManagerTwo = FrameTools.MsgSpan * 7,
}