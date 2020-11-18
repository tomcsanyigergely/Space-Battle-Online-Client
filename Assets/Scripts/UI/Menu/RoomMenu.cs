using System.Collections;
using System.Collections.Generic;
using UdpMessages.ClientServerMessages;
using UdpMessages.ServerClientMessages;
using UdpMessages.Utilities;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UInt8 = System.Byte;
using System;

public class RoomMenu : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private NetworkHandler networkHandler;

    [SerializeField] private Canvas roomMenu;

    [SerializeField] private TextMeshProUGUI inviteOthersText;
    [SerializeField] private TextMeshProUGUI roomTokenText;
    [SerializeField] private Button selectTeamOneButton;
    [SerializeField] private Button selectTeamTwoButton;
    [SerializeField] private Button unselectTeamButton;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button leaveRoomButton;
    [SerializeField] private Button copyToClipboardButton;
    private TextMeshProUGUI leaveRoomButtonText;

    [SerializeField] private TextMeshProUGUI[] slotTexts;
    
    private class RoomUserData
    {
        public string username;
        public UInt8? selectedTeam;
    }

    private enum RoomUserRole { Host, Invited }

    private string username;
    private RoomUserRole roomUserRole;
    private UInt8 userIndex;
    private UInt64 roomToken;
    private RoomUserData[] users;

    private void Start()
    {
        leaveRoomButtonText = leaveRoomButton.GetComponentInChildren<TextMeshProUGUI>();

        selectTeamOneButton.onClick.AddListener(() => { SendSelectTeam(0); });

        selectTeamTwoButton.onClick.AddListener(() => { SendSelectTeam(1); });

        unselectTeamButton.onClick.AddListener(() => { SendSelectTeam(null); });

        startGameButton.onClick.AddListener(() => { SendStartRoom(); });

        leaveRoomButton.onClick.AddListener(() => { SendLeaveRoom(); });

        copyToClipboardButton.onClick.AddListener(() => { GUIUtility.systemCopyBuffer = Convert.ToString(roomToken); });
        
        UIUtilities.Hide(roomMenu);
    }

    public void Enter(string username, RoomCreatedMessage roomCreatedMessage)
    {
        Setup(username, RoomUserRole.Host);

        OnReceived(roomCreatedMessage);

        inviteOthersText.gameObject.SetActive(true);
        roomTokenText.gameObject.SetActive(true);
        copyToClipboardButton.gameObject.SetActive(true);

        UIUtilities.Show(roomMenu);
    }

    public void Enter(string username, RoomEnteredMessage roomEnteredMessage)
    {
        Setup(username, RoomUserRole.Invited);

        OnReceived(roomEnteredMessage);

        inviteOthersText.gameObject.SetActive(false);
        roomTokenText.gameObject.SetActive(false);
        copyToClipboardButton.gameObject.SetActive(false);

        UIUtilities.Show(roomMenu);
    }

    private void Setup(string username, RoomUserRole roomUserRole)
    {
        this.username = username;
        this.roomUserRole = roomUserRole;
        foreach(var slotText in slotTexts)
        {
            slotText.text = "";
        }

        switch (roomUserRole)
        {
            case RoomUserRole.Host:
                startGameButton.gameObject.SetActive(true);
                leaveRoomButtonText.text = "Delete Room";
                break;
            case RoomUserRole.Invited:
                startGameButton.gameObject.SetActive(false);
                leaveRoomButtonText.text = "Leave Room";
                break;
        }
    }

    public void Exit()
    {
        UIUtilities.Hide(roomMenu);
    }

    private void SendSelectTeam(UInt8? selectedTeam)
    {
        if (selectedTeam.HasValue)
        {
            var selectedTeamOffset = UInt8Struct.CreateUInt8Struct(networkHandler.builder, selectedTeam.Value);
            SelectTeamMessage.StartSelectTeamMessage(networkHandler.builder);
            SelectTeamMessage.AddSelectedTeam(networkHandler.builder, selectedTeamOffset);            
        }
        else
        {
            SelectTeamMessage.StartSelectTeamMessage(networkHandler.builder);
        }

        var message = SelectTeamMessage.EndSelectTeamMessage(networkHandler.builder);

        networkHandler.Send(message);
    }

    private void SendStartRoom()
    {
        StartRoomMessage.StartStartRoomMessage(networkHandler.builder);
        var message = StartRoomMessage.EndStartRoomMessage(networkHandler.builder);

        networkHandler.Send(message);
    }

    private void SendLeaveRoom()
    {
        LeaveRoomMessage.StartLeaveRoomMessage(networkHandler.builder);
        var message = LeaveRoomMessage.EndLeaveRoomMessage(networkHandler.builder);

        networkHandler.Send(message);
    }

    private void UpdateSlotTexts()
    {
        UInt8 numberOfEnteredUsers = 0;
        foreach (var user in users)
        {
            if (user != null)
            {
                numberOfEnteredUsers++;
            }
        }

        UInt8 nextTeamOneSlotIndex = 0;
        UInt8 nextTeamTwoSlotIndex = Convert.ToByte(numberOfEnteredUsers - 1);
        for (UInt8 i = 0; i < users.Length; i++)
        {
            var user = users[i];

            if (user != null)
            {
                if (user.selectedTeam.HasValue)
                {
                    if (user.selectedTeam.Value == 0)
                    {
                        slotTexts[nextTeamOneSlotIndex].text = user.username + (i == userIndex ? " <" : "");
                        slotTexts[nextTeamOneSlotIndex].color = new Color(0, 255.0f / 255.0f, 246.0f / 255.0f);
                        nextTeamOneSlotIndex++;
                    }
                    else
                    {
                        slotTexts[nextTeamTwoSlotIndex].text = user.username + (i == userIndex ? " <" : "");
                        slotTexts[nextTeamTwoSlotIndex].color = Color.red;
                        nextTeamTwoSlotIndex--;
                    }
                }
            }
        }

        UInt8 nextEmptySlotIndex = numberOfEnteredUsers;
        UInt8 nextUnselectedTeamSlotIndex = nextTeamOneSlotIndex;

        for (UInt8 i = 0; i < users.Length; i++)
        {
            var user = users[i];

            if (user == null)
            {
                slotTexts[nextEmptySlotIndex].color = Color.white;
                slotTexts[nextEmptySlotIndex].text = "--";
                nextEmptySlotIndex++;
            }
            else if (user.selectedTeam == null)
            {
                slotTexts[nextUnselectedTeamSlotIndex].color = Color.white;
                slotTexts[nextUnselectedTeamSlotIndex].text = user.username + (i == userIndex ? " <" : "");
                nextUnselectedTeamSlotIndex++;
            }
        }
    }

    public void OnReceived(RoomCreatedMessage roomCreatedMessage)
    {
        userIndex = 0;
        roomToken = roomCreatedMessage.RoomToken;

        users = new RoomUserData[2 * roomCreatedMessage.TeamSize];
        users[0] = new RoomUserData();
        users[0].username = username;
        users[0].selectedTeam = null;

        roomTokenText.text = Convert.ToString(roomToken);

        copyToClipboardButton.gameObject.SetActive(true);

        UpdateSlotTexts();
    }

    public void OnReceived(RoomEnteredMessage roomEnteredMessage)
    {
        userIndex = roomEnteredMessage.UserIndex;

        users = new RoomUserData[2 * roomEnteredMessage.TeamSize];

        for (UInt8 i = 0; i < roomEnteredMessage.UsersLength; i++)
        {
            UdpMessages.ServerClientMessages.RoomUserData user = roomEnteredMessage.Users(i).Value;

            users[user.UserIndex] = new RoomUserData();
            users[user.UserIndex].username = user.Username;

            if (user.SelectedTeam.HasValue)
            {
                users[user.UserIndex].selectedTeam = user.SelectedTeam.Value.Value;
            }
            else
            {
                users[user.UserIndex].selectedTeam = null;
            }
        }

        UpdateSlotTexts();        
    }

    public void OnReceived(UserEnteredRoomMessage userEnteredRoomMessage)
    {
        UInt8 userIndex = userEnteredRoomMessage.UserIndex;
        users[userIndex] = new RoomUserData();
        users[userIndex].username = userEnteredRoomMessage.Username;
        users[userIndex].selectedTeam = null;

        UpdateSlotTexts();
    }

    public void OnReceived(UserLeftRoomMessage userLeftRoomMessage)
    {
        if (userLeftRoomMessage.UserIndex != userIndex)
        {
            users[userLeftRoomMessage.UserIndex] = null;

            UpdateSlotTexts();
        }
        else
        {
            LeaveRoomMenu();
        }
    }

    public void OnReceived(RoomDeletedMessage roomDeletedMessage)
    {
        LeaveRoomMenu();
    }

    public void OnReceived(UserSelectedTeamMessage userSelectedTeamMessage)
    {
        if (userSelectedTeamMessage.SelectedTeam.HasValue)
        {
            users[userSelectedTeamMessage.UserIndex].selectedTeam = userSelectedTeamMessage.SelectedTeam.Value.Value;
        }
        else
        {
            users[userSelectedTeamMessage.UserIndex].selectedTeam = null;
        }

        UpdateSlotTexts();
    }

    public void OnReceived(MatchInitializedMessage matchInitializedMessage)
    {
        gameSystem.ClientTokenScreen(matchInitializedMessage);
    }

    private void LeaveRoomMenu()
    {
        gameSystem.SelectPlayersMenu(username);
    }
}
