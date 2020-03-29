using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NetworkUtils : MonoBehaviour
{
    /// <summary>Serialize a byte in the sent packet.</summary>
    /// <param name="byteList">The byte list or packet where the byte will be added.</param>
    /// <param name="data">The byte to add.</param>
    public static void SerializeByte(List<byte> byteList, byte data)
    {
        byteList.Add(data);
    }

    /// <summary>Serialize a list of byte in the sent packet.</summary>
    /// <param name="byteList">The byte list or packet where the byte will be added.</param>
    /// <param name="data">The list of bytes to add.</param>
    public static void SerializeByteArray(List<byte> byteList, byte[] data)
    {
        byteList.AddRange(data);
    }

    /// <summary>Serialize a bool in the sent packet.</summary>
    /// <param name="byteList">The byte list or packet where the byte will be added.</param>
    /// <param name="data">The bool to add.</param>
    public static void SerializeBool(List<byte> byteList, bool data)
    {
        byteList.AddRange(BitConverter.GetBytes(data));
    }

    /// <summary>Serialize a short in the sent packet.</summary>
    /// <param name="byteList">The byte list or packet where the byte will be added.</param>
    /// <param name="data">The short to add.</param>
    public static void SerializeShort(List<byte> byteList, short data)
    {
        byteList.AddRange(BitConverter.GetBytes(data));
    }

    /// <summary>Serialize a ushort in the sent packet.</summary>
    /// <param name="byteList">The byte list or packet where the byte will be added.</param>
    /// <param name="data">The ushort to add.</param>
    public static void SerializeUshort(List<byte> byteList, ushort data)
    {
        byteList.AddRange(BitConverter.GetBytes(data));
    }

    /// <summary>Serialize a int in the sent packet.</summary>
    /// <param name="byteList">The byte list or packet where the byte will be added.</param>
    /// <param name="data">The int to add.</param>
    public static void SerializeInt(List<byte> byteList, int data)
    {
        byteList.AddRange(BitConverter.GetBytes(data));
    }

    /// <summary>Serialize a long in the sent packet.</summary>
    /// <param name="byteList">The byte list or packet where the byte will be added.</param>
    /// <param name="data">The long to add.</param>
    public static void SerializeLong(List<byte> byteList, long data)
    {
        byteList.AddRange(BitConverter.GetBytes(data));
    }

    /// <summary>Serialize a float in the sent packet.</summary>
    /// <param name="byteList">The byte list or packet where the byte will be added.</param>
    /// <param name="data">The float to add.</param>
    public static void SerializeFloat(List<byte> byteList, float data)
    {
        byteList.AddRange(BitConverter.GetBytes(data));
    }

    /// <summary>Serialize a string in the sent packet.</summary>
    /// <param name="byteList">The byte list or packet where the byte will be added.</param>
    /// <param name="data">The string to add.</param>
    public static void SerializeString(List<byte> byteList, string data)
    {
        SerializeInt(byteList, data.Length);
        byteList.AddRange(Encoding.ASCII.GetBytes(data));
    }

    /// <summary>Serialize a Vector2 in the sent packet.</summary>
    /// <param name="byteList">The byte list or packet where the byte will be added.</param>
    /// <param name="data">The Vector2 to add.</param>
    public static void SerializeVector2(List<byte> byteList, Vector2 data)
    {
        byteList.AddRange(BitConverter.GetBytes(data.x));
        byteList.AddRange(BitConverter.GetBytes(data.y));
    }

    /// <summary>Serialize a Vector3 in the sent packet.</summary>
    /// <param name="byteList">The byte list or packet where the byte will be added.</param>
    /// <param name="data">The Vector3 to add.</param>
    public static void SerializeVector3(List<byte> byteList, Vector3 data)
    {
        byteList.AddRange(BitConverter.GetBytes(data.x));
        byteList.AddRange(BitConverter.GetBytes(data.y));
        byteList.AddRange(BitConverter.GetBytes(data.z));
    }

    /// <summary>Deserialize a byte in the sent packet.</summary>
    /// <param name="data">The byte list or packet where the byte will be added.</param>
    /// <param name="offset">The data's read position.</param>
    public static byte DeserializeByte(byte[] data, ref int offset)
    {
        byte ret = data[offset];
        offset += sizeof(byte);
        return ret;
    }

    /// <summary>Deserialize a bool in the sent packet.</summary>
    /// <param name="data">The byte list or packet where the byte will be added.</param>
    /// <param name="offset">The data's read position.</param>
    public static bool DeserializeBool(byte[] data, ref int offset)
    {
        bool ret = BitConverter.ToBoolean(data, offset);
        offset += sizeof(bool);
        return ret;
    }

    /// <summary>Deserialize a short in the sent packet.</summary>
    /// <param name="data">The byte list or packet where the byte will be added.</param>
    /// <param name="offset">The data's read position.</param>
    public static short DeserializeShort(byte[] data, ref int offset)
    {
        short ret = BitConverter.ToInt16(data, offset);
        offset += sizeof(short);
        return ret;
    }

    /// <summary>Deserialize a ushort in the sent packet.</summary>
    /// <param name="data">The byte list or packet where the byte will be added.</param>
    /// <param name="offset">The data's read position.</param>
    public static ushort DeserializeUshort(byte[] data, ref int offset)
    {
        ushort ret = BitConverter.ToUInt16(data, offset);
        offset += sizeof(ushort);
        return ret;
    }

    /// <summary>Deserialize a int in the sent packet.</summary>
    /// <param name="data">The byte list or packet where the byte will be added.</param>
    /// <param name="offset">The data's read position.</param>
    public static int DeserializeInt(byte[] data, ref int offset)
    {
        int ret = BitConverter.ToInt32(data, offset);
        offset += sizeof(int);
        return ret;
    }

    /// <summary>Deserialize a long in the sent packet.</summary>
    /// <param name="data">The byte list or packet where the byte will be added.</param>
    /// <param name="offset">The data's read position.</param>
    public static long DeserializeLong(byte[] data, ref int offset)
    {
        long ret = BitConverter.ToInt64(data, offset);
        offset += sizeof(long);
        return ret;
    }

    /// <summary>Deserialize a float in the sent packet.</summary>
    /// <param name="data">The byte list or packet where the byte will be added.</param>
    /// <param name="offset">The data's read position.</param>
    public static float DeserializeFloat(byte[] data, ref int offset)
    {
        float ret = BitConverter.ToSingle(data, offset);
        offset += sizeof(float);
        return ret;
    }

    /// <summary>Deserialize a string in the sent packet.</summary>
    /// <param name="data">The byte list or packet where the byte will be added.</param>
    /// <param name="offset">The data's read position.</param>
    public static string DeserializeString(byte[] data, ref int offset)
    {
        int length = DeserializeInt(data, ref offset);
        string ret = Encoding.ASCII.GetString(data, offset, length);
        offset += ASCIIEncoding.ASCII.GetByteCount(ret);
        return ret;
    }

    /// <summary>Deserialize a Vector2 in the sent packet.</summary>
    /// <param name="data">The byte list or packet where the byte will be added.</param>
    /// <param name="offset">The data's read position.</param>
    public static Vector2 DeserializeVector2(byte[] data, ref int offset)
    {
        Vector2 ret = Vector2.zero;
        ret.x = BitConverter.ToSingle(data, offset);
        offset += sizeof(float);
        ret.y = BitConverter.ToSingle(data, offset);
        offset += sizeof(float);
        return ret;
    }

    /// <summary>Deserialize a Vector3 in the sent packet.</summary>
    /// <param name="data">The byte list or packet where the byte will be added.</param>
    /// <param name="offset">The data's read position.</param>
    public static Vector3 DeserializeVector3(byte[] data, ref int offset)
    {
        Vector3 ret = Vector3.zero;
        ret.x = BitConverter.ToSingle(data, offset);
        offset += sizeof(float);
        ret.y = BitConverter.ToSingle(data, offset);
        offset += sizeof(float);
        ret.z = BitConverter.ToSingle(data, offset);
        offset += sizeof(float);
        return ret;
    }
}

