using UnityEngine;
using System.Collections;
using System.IO;
//using ProtoBuf;

// ****************************************************************
// 功能：
// 创建：#SMARTDEVELOPERS#
// 时间：#CREATIONDATE#
// 修改内容：										修改者姓名：
// ****************************************************************

public class IProtoTools {

    ///// <summary>
    ///// 序列化出来给socket发送
    ///// </summary>
    ///// <param name="msg"></param>
    ///// <returns></returns>
    //public static byte[] Serialize(IExtensible msg) {
    //    byte[] result;

    //    using (var stream = new MemoryStream()) {
    //        Serializer.Serialize(stream, msg);

    //        result = stream.ToArray();
    //    }

    //    return result;
    //}


    ///// <summary>
    ///// 主要从socket接收数据，反序列化成类
    ///// </summary>
    ///// <typeparam name="IExtensible"></typeparam>
    ///// <param name="message"></param>
    ///// <returns></returns>
    //public static IExtensible Deserizlize<IExtensible>(byte[] message) {
    //    IExtensible result;

    //    using (var stream = new MemoryStream(message)) {
    //        result = Serializer.Deserialize<IExtensible>(stream);
    //    }
    //    return result;
    //}
}

