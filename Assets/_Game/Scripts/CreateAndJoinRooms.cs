using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Linq;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    [SerializeField] private GameObject inGameUI;

    private Dictionary<string, RoomInfo> availableRooms = new Dictionary<string, RoomInfo>();
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
    }
    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(createInput.text, options);
        }
        else
        {
            Debug.Log("Not connected to Photon Master Server");
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room created failed: " + message, this);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        availableRooms.Clear();
        foreach (RoomInfo room in roomList)
        {
            availableRooms[room.Name] = room;
        }
    }
    public void JoinRoom()
    {
        string roomName = joinInput.text;
        if (roomName != "")
        {
            if (availableRooms.ContainsKey(roomName))
            {
                // Room exists, join it
                PhotonNetwork.JoinRoom(roomName);
            }
            else
            {
                Debug.Log("Room does not exist or is full: " + roomName);
            }
        }
        else
        {
            Debug.Log("Invalid room name. Please enter a valid room name.");
        }
    }
    public void JoinSinglePlayerRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 1;
            PhotonNetwork.CreateRoom("singleplayer", options);
        }
        else
        {
            Debug.Log("Not connected to Photon Master Server");
        }
    }
    public override void OnJoinedRoom()
    {
        LevelManager.Instance.OnPlay();
        UIManager.Instance.ChangeUI(inGameUI);
        Debug.Log("A player joined the room");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room: " + message);
    }
    public void LeaveRoom()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            Debug.LogWarning("Cannot leave the room. Client is not connected or not in a room.");
        }
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Left the room");
    }
}
