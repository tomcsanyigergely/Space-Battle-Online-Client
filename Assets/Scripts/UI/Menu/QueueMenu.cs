using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UdpMessages.ServerClientMessages;
using UdpMessages.ClientServerMessages;
using UInt8 = System.Byte;

public class QueueMenu : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private NetworkHandler networkHandler;

    [SerializeField] private Canvas queueMenu;

    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI queueStateText;

    private string username;
    private UInt8 teamSize;
    private bool leaving;

    // Start is called before the first frame update
    private void Start()
    {
        cancelButton.onClick.AddListener(() => {
            this.leaving = true;
            LeaveQueueMessage.StartLeaveQueueMessage(networkHandler.builder);
            var message = LeaveQueueMessage.EndLeaveQueueMessage(networkHandler.builder);
            networkHandler.Send(message);
        });

        UIUtilities.Hide(queueMenu);
    }

    public void Enter(string username, UInt8 teamSize, bool leaving, QueueUpdateMessage queueUpdateMessage)
    {
        this.username = username;
        this.teamSize = teamSize;
        this.leaving = leaving;

        OnReceived(queueUpdateMessage);

        UIUtilities.Show(queueMenu);
    }

    public void Exit()
    {
        UIUtilities.Hide(queueMenu);
    }    

    public void OnReceived(QueueUpdateMessage queueUpdateMessage)
    {
        queueStateText.text = "Players assembled: " + queueUpdateMessage.PlayersInQueue + " / " + 2*teamSize;
    }

    public void OnReceived(QueueLeftMessage queueLeftMessage)
    {
        if (this.leaving)
        {
            gameSystem.SelectGameModeMenu(username, GameSystem.Players.Random);
        }
    }

    public void OnReceived(MatchInitializedMessage matchInitializedMessage)
    {
        gameSystem.ClientTokenScreen(matchInitializedMessage);
    }
}
