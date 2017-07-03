using UnityEngine;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class GloableConfig {
    public string ipAddress = "127.0.0.1";
    public ushort port = 1025;
    public bool isNetPlayer = true;
}

public enum UIEvent {
    CheckInitialFinish = ManagerID.UIManager + 1,
}
