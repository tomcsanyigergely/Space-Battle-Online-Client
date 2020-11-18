using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UdpMessages.ServerClientMessages;
using UdpMessages.ClientServerMessages;

public class EnteringMatchScreen : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private NetworkHandler networkHandler;

    [SerializeField] private Canvas enteringMatchScreen;

    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button continueButton;

    private UInt64 clientToken;
    private float timeToEnterMatch;
    private bool entering;

    private void Start()
    {
        continueButton.onClick.AddListener(() => { gameSystem.ReconnectionMenu(); });

        UIUtilities.Hide(enteringMatchScreen);
    }

    public void Enter(UInt64 clientToken)
    {
        this.clientToken = clientToken;
        this.timeToEnterMatch = 0.5f;
        this.entering = true;

        messageText.text = "Entering match...";
        continueButton.gameObject.SetActive(false); 

        UIUtilities.Show(enteringMatchScreen);

        EnterMatch();
    }

    public void Exit()
    {
        UIUtilities.Hide(enteringMatchScreen);
    }

    // Called each frame from GameSystem
    public void MenuUpdate()
    {
        if (entering)
        {
            timeToEnterMatch -= Time.deltaTime;
            if (timeToEnterMatch <= 0)
            {
                EnterMatch();
                do
                {
                    timeToEnterMatch += 0.5f;
                } while (timeToEnterMatch <= 0);
            }
        }
    }

    private void EnterMatch()
    {
        ConnectMessage.StartConnectMessage(networkHandler.builder);
        ConnectMessage.AddFix(networkHandler.builder, ConnectMessageFix.CreateConnectMessageFix(networkHandler.builder, clientToken));
        var connectMessage = ConnectMessage.EndConnectMessage(networkHandler.builder);
        networkHandler.Send(connectMessage);
    }

    public void OnReceived(InitGameMessage initGameMessage)
    {
        gameSystem.InGame(initGameMessage);
    }

    public void OnReceived(InvalidClientTokenMessage invalidClientTokenMessage)
    {
        this.entering = false;
        messageText.text = "Invalid match token!";
        continueButton.gameObject.SetActive(true);
    }
}
