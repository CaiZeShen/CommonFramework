using System;
using UnityEngine;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class LuaEventProcess : MonoBase {
    private static LuaEventProcess instance;
    private MonoBase monoChild = null;

    public static LuaEventProcess Instance {
        get {
            return instance;
        }
    }

    public override void ProcessEvent(MsgBase msg) {
        if (monoChild !=null) {
            monoChild.ProcessEvent(msg);
        }
    }

    public void SettingChild(MonoBase child) {
        this.monoChild = child;
    }

    private void Awake() {
        instance = this;
    }
}

