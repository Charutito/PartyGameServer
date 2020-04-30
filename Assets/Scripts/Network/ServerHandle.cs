using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
        Server.clients[_fromClient].SendIntoGame(_username);
        Server.clients[_fromClient].SendMapArenaToClient();
        Server.clients[_fromClient].SendPickupableSkillsOnMapToClient();
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        float[] _inputs = new float[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadFloat();
        }

        Server.clients[_fromClient].player.SetInput(_inputs);
    }

    public static void PlayerShoot(int _fromClient, Packet _packet)
    {
        Vector3 _shootDirection = _packet.ReadVector3();
        string _skillId = _packet.ReadString();

        Server.clients[_fromClient].player.Shoot(_shootDirection, _skillId);
    }

    public static void PlayerSkillRotation(int _fromClient, Packet _packet)
    {
        Vector3 _worldPosition = _packet.ReadVector3();
        Server.clients[_fromClient].player.SetSkillRotation(_worldPosition);
    }
}
