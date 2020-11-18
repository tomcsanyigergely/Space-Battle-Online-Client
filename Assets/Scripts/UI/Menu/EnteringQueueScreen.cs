using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UInt8 = System.Byte;
using UdpMessages.ServerClientMessages;
using UdpMessages.ClientServerMessages;

public class EnteringQueueScreen : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private NetworkHandler networkHandler;

    [SerializeField] private Canvas enteringQueueScreen;

    [SerializeField] private Button cancelButton;

    private string username;
    private UInt8 teamSize;
    private bool leaving;

    private void Start()
    {
        cancelButton.onClick.AddListener(() => {
            this.leaving = true;
            LeaveQueueMessage.StartLeaveQueueMessage(networkHandler.builder);
            var message = LeaveQueueMessage.EndLeaveQueueMessage(networkHandler.builder);
            networkHandler.Send(message);
        });

        UIUtilities.Hide(enteringQueueScreen);
    }

    public void Enter(string username, UInt8 teamSize)
    {
        this.username = username;
        this.teamSize = teamSize;
        this.leaving = false;

        SendEnterQueueMessage();

        UIUtilities.Show(enteringQueueScreen);
    }

    public void Exit()
    {
        UIUtilities.Hide(enteringQueueScreen);
    }

    private void SendEnterQueueMessage()
    {
        var usernameOffset = networkHandler.builder.CreateString(username);
        EnterQueueMessage.StartEnterQueueMessage(networkHandler.builder);
        EnterQueueMessage.AddTeamSize(networkHandler.builder, teamSize);
        EnterQueueMessage.AddUsername(networkHandler.builder, usernameOffset);
        var message = EnterQueueMessage.EndEnterQueueMessage(networkHandler.builder);

        networkHandler.Send(message);
    }

    public void OnReceived(MatchInitializedMessage matchInitializedMessage)
    {
        gameSystem.ClientTokenScreen(matchInitializedMessage);
    }

    public void OnReceived(QueueUpdateMessage queueUpdateMessage)
    {
        gameSystem.QueueMenu(username, teamSize, leaving, queueUpdateMessage);
    }

    public void OnReceived(QueueLeftMessage queueLeftMessage)
    {
        if (this.leaving)
        {
            gameSystem.SelectGameModeMenu(username, GameSystem.Players.Random);
        }
    }
}
