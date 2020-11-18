using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReconnectionMenu : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;

    [SerializeField] private Canvas reconnectionMenu;

    [SerializeField] private TMP_InputField clientTokenInputField;
    [SerializeField] private Button reconnectButton;
    [SerializeField] private Button cancelButton;

    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField, TextArea] private string clientTokenErrorText;

    private void Start()
    {
        reconnectButton.onClick.AddListener(() => { EnterGame(); });

        cancelButton.onClick.AddListener(() => { gameSystem.MainMenu(); });

        UIUtilities.Hide(reconnectionMenu);
    }

    public void Enter()
    {
        clientTokenInputField.text = "";
        errorText.text = "";

        UIUtilities.Show(reconnectionMenu);
    }

    public void Exit()
    {
        UIUtilities.Hide(reconnectionMenu);
    }

    private void EnterGame()
    {
        errorText.text = "";

        if (clientTokenInputField.text.Length > 0)
        {
            UInt64 clientToken = Convert.ToUInt64(clientTokenInputField.text);            

            gameSystem.EnteringMatchScreen(clientToken);
        }
        else
        {
            errorText.text += clientTokenErrorText;
        }
    }
}
