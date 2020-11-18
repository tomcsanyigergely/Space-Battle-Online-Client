using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UdpMessages.ClientServerMessages;
using System;

public class EnterRoomMenu : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private NetworkHandler networkHandler;

    [SerializeField] private Canvas enterRoomMenu;

    [SerializeField] private TMP_InputField roomTokenInputField;
    [SerializeField] private Button enterRoomButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI errorText;

    [SerializeField, TextArea] private string roomTokenErrorText;

    private string username;

    private void Start()
    {
        enterRoomButton.onClick.AddListener(() => {
            SendEnterRoom();
        });

        cancelButton.onClick.AddListener(() => {
            gameSystem.SelectPlayersMenu(username);
        });

        UIUtilities.Hide(enterRoomMenu);
    }

    public void Enter(string username)
    {
        this.username = username;
        roomTokenInputField.text = "";
        errorText.text = "";
        UIUtilities.Show(enterRoomMenu);
    }

    public void Exit()
    {
        UIUtilities.Hide(enterRoomMenu);
    }

    private void SendEnterRoom()
    {
        errorText.text = "";

        bool validRoomTokenField = roomTokenInputField.text.Length != 0;

        if (validRoomTokenField)
        {
            UInt64 roomToken = Convert.ToUInt64(roomTokenInputField.text);            

            gameSystem.EnteringRoomScreen(username, roomToken);
        }
        else
        {
            errorText.text += roomTokenErrorText + "\n";
        }
    }

}
