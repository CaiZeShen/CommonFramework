using System;
using UnityEngine;
using LuaInterface;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class LuaAndCMsgCenter : MonoBase {
    private static LuaAndCMsgCenter instance;
    private LuaFunction callback = null;
    
    public static LuaAndCMsgCenter Instance {
        get {
            return instance;
        }
    }

    public override void ProcessEvent(MsgBase msg) {
        if (callback == null) {
            return;
        }

        // 从网络来的msg 和从框架其他模块来的数据不一样
        if (msg.GetState() != 127) {                // 从网络来的
            NetMsgBase netMsg = (NetMsgBase)msg;

            byte[] proto = netMsg.GetProtoBuffer();

            // 转成lua能识别的字符串
            LuaByteBuffer buffer = new LuaByteBuffer(proto);

            // 第一个参数代表是否是网络来的
            callback.Call(true, netMsg.msgID, netMsg.GetState(), buffer);
        } else {                                    // 从框架来的
            callback.Call(false, msg);
        }
    }

    // 需要lua 注册回调函数
    public void SettingLuaCallBack(LuaFunction luafunc) {
        callback = luafunc;
    }

    private void Awake() {
        instance = this;
    }
}

