using UnityEngine;
using System.Diagnostics;
using UnityEditor;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class PythonTools {

    public static string PythonPath {
        get {
            string dataPath = Application.dataPath;
            string tmpPath = dataPath.Substring(0, dataPath.IndexOf("Assets"));
            return tmpPath + "ITools/Python/";
        }
    }

    [MenuItem("Itools/TestPython")]
    public static void TestPython() {
        string fileName = PythonPath + "lunch.bat";
        Process pro = Process.Start(fileName);
        pro.WaitForExit();
    }
}

