using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdpMessages.ServerClientMessages;
using UdpMessages.ClientServerMessages;
using UInt8 = System.Byte;

public class CreatingRoomScreen : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private NetworkHandler networkHandler;

    [SerializeField] private Canvas creatingRoomScreen;

    private string username;
    private UInt8 teamSize;
    private float timeToCreateRoom;

    private void Start()
    {
        UIUtilities.Hide(creatingRoomScreen);
    }

    public void Enter(string username, UInt8 teamSize)
    {
        this.username = username;
        this.teamSize = teamSize;
        this.timeToCreateRoom = 0.5f;

        UIUtilities.Show(creatingRoomScreen);

        CreateRoom();
    }

    public void Exit()
    {
        UIUtilities.Hide(creatingRoomScreen);
    }

    // Called each frame from GameSystem
    public void MenuUpdate()
    {
        timeToCreateRoom -= Time.deltaTime;
        if (timeToCreateRoom <= 0)
        {
            CreateRoom();
            do
            {
                timeToCreateRoom += 0.5f;
            } while (timeToCreateRoom <= 0);
        }
    }

    private void CreateRoom()
    {
        var usernameOffset = networkHandler.builder.CreateString(username);

        CreateRoomMessage.StartCreateRoomMessage(networkHandler.builder);
        CreateRoomMessage.AddTeamSize(networkHandler.builder, teamSize);
        CreateRoomMessage.AddUsername(networkHandler.builder, usernameOffset);
        var message = CreateRoomMessage.EndCreateRoomMessage(networkHandler.builder);

        networkHandler.Send(message);
    }

    public void OnReceived(RoomCreatedMessage roomCreatedMessage)
    {
        gameSystem.RoomMenu(username, roomCreatedMessage);
    }
}
