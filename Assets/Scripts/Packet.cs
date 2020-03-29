using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Sent from server to client.</summary>
public enum ServerPackets
{
    welcome = 1,
    spawnPlayer,
    playerPosition,
    playerRotation,
    playerDisconnected,
    playerHealth,
    playerRespawned,
    createItemSpawner,
    itemSpawned,
    itemPickedUp
}

/// <summary>Sent from client to server.</summary>
public enum ClientPackets
{
    welcomeReceived = 1,
    playerMovement,
    playerShoot
}

public class Packet : IDisposable
{
    private List<byte> buffer;
    private byte[] readableBuffer;
    private int readPos;

    /// <summary>Creates a new empty packet (without an ID).</summary>
    public Packet()
    {
        buffer = new List<byte>(); // Initialize buffer
        readPos = 0; // Set readPos to 0
    }

    /// <summary>Creates a new packet with a given ID. Used for sending.</summary>
    /// <param name="_id">The packet ID.</param>
    public Packet(int _id)
    {
        buffer = new List<byte>(); // Initialize buffer
        readPos = 0; // Set readPos to 0

        Write(_id); // Write packet id to the buffer
    }

    /// <summary>Creates a packet from which data can be read. Used for receiving.</summary>
    /// <param name="_data">The bytes to add to the packet.</param>
    public Packet(byte[] _data)
    {
        buffer = new List<byte>(); // Initialize buffer
        readPos = 0; // Set readPos to 0

        SetBytes(_data);
    }

    #region Functions
    /// <summary>Sets the packet's content and prepares it to be read.</summary>
    /// <param name="_data">The bytes to add to the packet.</param>
    public void SetBytes(byte[] _data)
    {
        Write(_data);
        readableBuffer = buffer.ToArray();
    }

    /// <summary>Inserts the length of the packet's content at the start of the buffer.</summary>
    public void WriteLength()
    {
        buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count)); // Insert the byte length of the packet at the very beginning
    }

    /// <summary>Inserts the given int at the start of the buffer.</summary>
    /// <param name="_value">The int to insert.</param>
    public void InsertInt(int _value)
    {
        buffer.InsertRange(0, BitConverter.GetBytes(_value)); // Insert the int at the start of the buffer
    }

    /// <summary>Gets the packet's content in array form.</summary>
    public byte[] ToArray()
    {
        readableBuffer = buffer.ToArray();
        return readableBuffer;
    }

    /// <summary>Gets the length of the packet's content.</summary>
    public int Length()
    {
        return buffer.Count; // Return the length of buffer
    }

    /// <summary>Gets the length of the unread data contained in the packet.</summary>
    public int UnreadLength()
    {
        return Length() - readPos; // Return the remaining length (unread)
    }

    /// <summary>Resets the packet instance to allow it to be reused.</summary>
    /// <param name="_shouldReset">Whether or not to reset the packet.</param>
    public void Reset(bool _shouldReset = true)
    {
        if (_shouldReset)
        {
            buffer.Clear(); // Clear buffer
            readableBuffer = null;
            readPos = 0; // Reset readPos
        }
        else
        {
            readPos -= 4; // "Unread" the last read int
        }
    }
    #endregion

    #region Write Data
    /// <summary>Adds a byte to the packet.</summary>
    /// <param name="_value">The byte to add.</param>
    public void Write(byte _value)
    {
        buffer.Add(_value);
    }
    /// <summary>Adds an array of bytes to the packet.</summary>
    /// <param name="_value">The byte array to add.</param>
    public void Write(byte[] _value)
    {
        buffer.AddRange(_value);
    }
    /// <summary>Adds a short to the packet.</summary>
    /// <param name="_value">The short to add.</param>
    public void Write(short _value)
    {
        NetworkUtils.SerializeShort(buffer, _value);
    }
    /// <summary>Adds an int to the packet.</summary>
    /// <param name="_value">The int to add.</param>
    public void Write(int _value)
    {
        NetworkUtils.SerializeInt(buffer, _value);
    }
    /// <summary>Adds a long to the packet.</summary>
    /// <param name="_value">The long to add.</param>
    public void Write(long _value)
    {
        NetworkUtils.SerializeLong(buffer, _value);
    }
    /// <summary>Adds a float to the packet.</summary>
    /// <param name="_value">The float to add.</param>
    public void Write(float _value)
    {
        NetworkUtils.SerializeFloat(buffer, _value);
    }
    /// <summary>Adds a bool to the packet.</summary>
    /// <param name="_value">The bool to add.</param>
    public void Write(bool _value)
    {
        NetworkUtils.SerializeBool(buffer, _value);
    }
    /// <summary>Adds a string to the packet.</summary>
    /// <param name="_value">The string to add.</param>
    public void Write(string _value)
    {
        NetworkUtils.SerializeString(buffer, _value);
    }
    /// <summary>Adds a Vector3 to the packet.</summary>
    /// <param name="_value">The Vector3 to add.</param>
    public void Write(Vector3 _value)
    {
        NetworkUtils.SerializeVector3(buffer, _value);
    }
    #endregion

    #region Read Data
    /// <summary>Reads a byte from the packet.</summary>
    /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
    public byte ReadByte(bool _moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            // If there are unread bytes
            byte _value = readableBuffer[readPos]; // Get the byte at readPos' position
            if (_moveReadPos)
            {
                // If _moveReadPos is true
                readPos += 1; // Increase readPos by 1
            }
            return _value; // Return the byte
        }
        else
        {
            throw new Exception("Could not read value of type 'byte'!");
        }
    }

    /// <summary>Reads an array of bytes from the packet.</summary>
    /// <param name="_length">The length of the byte array.</param>
    /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
    public byte[] ReadBytes(int _length, bool _moveReadPos = true)
    {
        if (buffer.Count > readPos)
        {
            // If there are unread bytes
            byte[] _value = buffer.GetRange(readPos, _length).ToArray(); // Get the bytes at readPos' position with a range of _length
            if (_moveReadPos)
            {
                // If _moveReadPos is true
                readPos += _length; // Increase readPos by _length
            }
            return _value;
        }
        else
        {
            throw new Exception("Could not read value of type 'byte[]'!");
        }
    }

    /// <summary>Reads a short from the packet.</summary>
    public short ReadShort()
    {
        if (buffer.Count > readPos)
        {
            // If there are unread bytes
            short _value = NetworkUtils.DeserializeShort(readableBuffer, ref readPos);
            return _value;
        }
        else
        {
            throw new Exception("Could not read value of type 'short'!");
        }
    }

    /// <summary>Reads an int from the packet.</summary>
    public int ReadInt()
    {
        if (buffer.Count > readPos)
        {
            // If there are unread bytes
            int _value = NetworkUtils.DeserializeInt(readableBuffer, ref readPos); // Convert the bytes to an int
            return _value;
        }
        else
        {
            throw new Exception("Could not read value of type 'int'!");
        }
    }

    /// <summary>Reads a long from the packet.</summary>
    public long ReadLong()
    {
        if (buffer.Count > readPos)
        {
            // If there are unread bytes
            long _value = NetworkUtils.DeserializeLong(readableBuffer, ref readPos); // Convert the bytes to a long
            return _value;
        }
        else
        {
            throw new Exception("Could not read value of type 'long'!");
        }
    }

    /// <summary>Reads a float from the packet.</summary>
    public float ReadFloat()
    {
        if (buffer.Count > readPos)
        {
            // If there are unread bytes
            float _value = NetworkUtils.DeserializeFloat(readableBuffer, ref readPos); // Convert the bytes to a float
            return _value;
        }
        else
        {
            throw new Exception("Could not read value of type 'float'!");
        }
    }

    /// <summary>Reads a bool from the packet.</summary>
    public bool ReadBool()
    {
        if (buffer.Count > readPos)
        {
            // If there are unread bytes
            bool _value = NetworkUtils.DeserializeBool(readableBuffer, ref readPos); // Convert the bytes to a bool
            return _value;
        }
        else
        {
            throw new Exception("Could not read value of type 'bool'!");
        }
    }

    /// <summary>Reads a string from the packet.</summary>
    public string ReadString()
    {
        try
        {
            string _value = NetworkUtils.DeserializeString(readableBuffer, ref readPos);
            return _value;
        }
        catch
        {
            throw new Exception("Could not read value of type 'string'!");
        }
    }

    /// <summary>Reads a Vector2 from the packet.</summary>
    public Vector3 ReadVector2()
    {
        return NetworkUtils.DeserializeVector2(readableBuffer, ref readPos);
    }

    /// <summary>Reads a Vector3 from the packet.</summary>
    public Vector3 ReadVector3()
    {
        return NetworkUtils.DeserializeVector3(readableBuffer, ref readPos);
    }

    //TODO Remove this method after Player Movement refactor
    /// <summary>Reads a Quaternion from the packet.</summary>
    public Quaternion ReadQuaternion()
    {
        return new Quaternion(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
    }
    #endregion

    private bool disposed = false;

    protected virtual void Dispose(bool _disposing)
    {
        if (!disposed)
        {
            if (_disposing)
            {
                buffer = null;
                readableBuffer = null;
                readPos = 0;
            }

            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
