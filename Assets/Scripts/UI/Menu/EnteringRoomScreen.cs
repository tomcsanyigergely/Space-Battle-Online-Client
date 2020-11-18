using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UdpMessages.ServerClientMessages;
using UdpMessages.ClientServerMessages;

public class EnteringRoomScreen : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private NetworkHandler networkHandler;

    [SerializeField] private Canvas enteringRoomScreen;

    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button continueButton;

    private string username;
    private UInt64 roomToken;
    private float timeToEnterRoom;
    private bool entering;

    private void Start()
    {
        continueButton.onClick.AddListener(() => { gameSystem.EnterRoomMenu(username); });

        UIUtilities.Hide(enteringRoomScreen);
    }

    public void Enter(string username, UInt64 roomToken)
    {
        this.username = username;
        this.roomToken = roomToken;
        this.timeToEnterRoom = 0.5f;
        this.entering = true;

        messageText.text = "Entering room...";
        continueButton.gameObject.SetActive(false);

        UIUtilities.Show(enteringRoomScreen);

        EnterRoom();
    }

    public void Exit()
    {
        UIUtilities.Hide(enteringRoomScreen);
    }

    // Called each frame from GameSystem
    public void MenuUpdate()
    {
        if (entering)
        {
            timeToEnterRoom -= Time.deltaTime;
            if (timeToEnterRoom <= 0)
            {
                EnterRoom();
                do
                {
                    timeToEnterRoom += 0.5f;
                } while (timeToEnterRoom <= 0);
            }
        }
    }

    private void EnterRoom()
    {
        var usernameOffset = networkHandler.builder.CreateString(username);

        EnterRoomMessage.StartEnterRoomMessage(networkHandler.builder);
        EnterRoomMessage.AddRoomToken(networkHandler.builder, roomToken);
        EnterRoomMessage.AddUsername(networkHandler.builder, usernameOffset);
        var message = EnterRoomMessage.EndEnterRoomMessage(networkHandler.builder);

        networkHandler.Send(message);
    }

    public void OnReceived(RoomEnteredMessage roomEnteredMessage)
    {
        gameSystem.RoomMenu(username, roomEnteredMessage);
    }

    public void OnReceived(InvalidRoomTokenMessage invalidRoomTokenMessage)
    {
        this.entering = false;
        messageText.text = "Invalid room token!";
        continueButton.gameObject.SetActive(true);
    }
}
