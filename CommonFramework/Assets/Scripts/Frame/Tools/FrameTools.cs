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
    LuaManager = 0,
    LNetManager = FrameTools.MsgSpan * 1,
    LUIManager = FrameTools.MsgSpan * 2,
    LNPCManager = FrameTools.MsgSpan * 3,
    LCharactorManager = FrameTools.MsgSpan * 4,
    LAssetMananger = FrameTools.MsgSpan * 5,
    LDataManager = FrameTools.MsgSpan * 6,
    LAudioManager = FrameTools.MsgSpan * 7,

    NetManager = FrameTools.MsgSpan * 12,
    UIManager = FrameTools.MsgSpan * 13,
    NPCManager = FrameTools.MsgSpan * 14,
    CharactorManager = FrameTools.MsgSpan * 15,
    AssetMananger = FrameTools.MsgSpan * 16,
    DataManager = FrameTools.MsgSpan * 17,
    AudioManager = FrameTools.MsgSpan * 18,
}