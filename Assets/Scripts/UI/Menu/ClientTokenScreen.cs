using System;
using System.Collections;
using System.Collections.Generic;
using UdpMessages.ServerClientMessages;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UInt8 = System.Byte;

public class ClientTokenScreen : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private NetworkHandler networkHandler;

    [SerializeField] private Canvas clientTokenScreen;

    [SerializeField] private TextMeshProUGUI clientTokenText;
    [SerializeField] private TextMeshProUGUI yourTeamText;
    [SerializeField] private TextMeshProUGUI opponentTeamText;
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI continueButtonText;
    [SerializeField] private Button copyToClipboardButton;

    private UInt64 clientToken;

    private float countdown = 0;

    private void Start()
    {
        continueButton.onClick.AddListener(() => { EnterGame(); });

        copyToClipboardButton.onClick.AddListener(() => { GUIUtility.systemCopyBuffer = Convert.ToString(clientToken); });

        UIUtilities.Hide(clientTokenScreen);
    }

    private void Update()
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0)
            {
                EnterGame();
            }
            else
            {
                continueButtonText.text = "Continue\n(" + Mathf.CeilToInt(countdown) + ")";
            }
        }
    }

    public void Enter(MatchInitializedMessage matchInitializedMessage)
    {
        clientToken = matchInitializedMessage.ClientToken;
        clientTokenText.text = Convert.ToString(clientToken);

        countdown = 55.0f;

        continueButtonText.text = "Continue\n" + Mathf.CeilToInt(countdown);

        yourTeamText.text = "";
        opponentTeamText.text = "";

        UInt8 playerNumber = matchInitializedMessage.PlayerNumber;
        UInt8 teamSize = Convert.ToByte(matchInitializedMessage.TeamOnePlayersLength);

        if (playerNumber < teamSize) // player is in team one
        {
            UInt8 playerIndexInTeamOne = playerNumber;

            yourTeamText.text += matchInitializedMessage.TeamOnePlayers(playerIndexInTeamOne) + "\n";

            for (UInt8 i = 0; i < matchInitializedMessage.TeamOnePlayersLength; i++)
            {
                if (i != playerIndexInTeamOne)
                {
                    yourTeamText.text += matchInitializedMessage.TeamOnePlayers(i) + "\n";
                }
            }
            for (UInt8 i = 0; i < matchInitializedMessage.TeamTwoPlayersLength; i++)
            {
                opponentTeamText.text += matchInitializedMessage.TeamTwoPlayers(i) + "\n";
            }
        }
        else // player is in team two
        {
            UInt8 playerIndexInTeamTwo = Convert.ToByte(playerNumber % teamSize);

            yourTeamText.text += matchInitializedMessage.TeamTwoPlayers(playerIndexInTeamTwo) + "\n";

            for (UInt8 i = 0; i < matchInitializedMessage.TeamOnePlayersLength; i++)
            {
                opponentTeamText.text += matchInitializedMessage.TeamOnePlayers(i) + "\n";
            }
            for (UInt8 i = 0; i < matchInitializedMessage.TeamTwoPlayersLength; i++)
            {
                if (i != playerIndexInTeamTwo)
                {
                    yourTeamText.text += matchInitializedMessage.TeamTwoPlayers(i) + "\n";
                }
            }
        }

        UIUtilities.Show(clientTokenScreen);
    }

    public void Exit()
    {
        UIUtilities.Hide(clientTokenScreen);
    }

    private void EnterGame()
    {
        countdown = 0;
        gameSystem.EnteringMatchScreen(clientToken);
    }
}
