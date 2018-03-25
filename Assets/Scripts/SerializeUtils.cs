using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

public class SerializeUtils
{

    public static string Serialize<T>(T obj)
    {
        var b = new BinaryFormatter();
        var m = new MemoryStream();
        b.Serialize(m, obj);
        return Convert.ToBase64String(
            m.GetBuffer()
        );
    }

    public static T Deserialize<T>(string str)
    {
        var b = new BinaryFormatter();
        var m = new MemoryStream(Convert.FromBase64String(str));
        return (T)b.Deserialize(m);
    }
}
