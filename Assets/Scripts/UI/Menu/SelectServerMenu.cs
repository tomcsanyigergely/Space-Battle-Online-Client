using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectServerMenu : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private NetworkHandler networkHandler;

    [SerializeField] private Canvas selectServerMenu;

    [SerializeField] private TextMeshProUGUI selectServerText;
    [SerializeField] private TextMeshProUGUI establishingConnectionText;
    [SerializeField] private TextMeshProUGUI connectionFailedText;
    [SerializeField] private TextMeshProUGUI versionText;

    [SerializeField] private Button serverOneButton;
    [SerializeField] private Button connectionFailedOkButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button checkServerStatusButton;
    [SerializeField] private Button customServerButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button creditsButton;

    private bool connecting;

    private void Start()
    {
        versionText.text = "v" + Config.MAJOR_VERSION + "." + Config.MINOR_VERSION + "." + Config.PATCH_VERSION;

        serverOneButton.onClick.AddListener(() => {
            Connect("52.29.103.48", 10000);
        });

        connectionFailedOkButton.onClick.AddListener(() => {
            gameSystem.SelectServerMenu();
        });

        cancelButton.onClick.AddListener(() => {
            CancelConnection();
        });
        
        customServerButton.onClick.AddListener(() =>
        {
            gameSystem.CustomServerMenu();
        });

        exitButton.onClick.AddListener(() => Application.Quit());

        creditsButton.onClick.AddListener(() => { gameSystem.CreditsScreen(); });

        UIUtilities.Hide(selectServerMenu);

        gameSystem.SelectServerMenu();
    }

    public void Enter()
    {
        connecting = false;

        selectServerText.gameObject.SetActive(true);
        establishingConnectionText.gameObject.SetActive(false);
        connectionFailedText.gameObject.SetActive(false);

        serverOneButton.gameObject.SetActive(true);
        customServerButton.gameObject.SetActive(true);
        connectionFailedOkButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
        //checkServerStatusButton.gameObject.SetActive(true);

        exitButton.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(true);

        UIUtilities.Show(selectServerMenu);
    }

    public void Exit()
    {
        UIUtilities.Hide(selectServerMenu);
    }

    private void Connect(String ip, UInt16 port)
    {
        selectServerText.gameObject.SetActive(false);
        establishingConnectionText.gameObject.SetActive(true);
        serverOneButton.gameObject.SetActive(false);
        customServerButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(true);
        checkServerStatusButton.gameObject.SetActive(false);

        exitButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);

        connecting = true;

        networkHandler.Connect(ip, port);
    }

    private void CancelConnection()
    {
        if (connecting)
        {
            connecting = false;
            networkHandler.Disconnect();
            gameSystem.SelectServerMenu();
        }
    }

    public void OnConnected()
    {
        if (connecting)
        {
            connecting = false;
            gameSystem.VersionScreen();
        }
    }

    public void OnDisconnected()
    {
        connecting = false;

        establishingConnectionText.gameObject.SetActive(false);
        connectionFailedText.gameObject.SetActive(true);

        connectionFailedOkButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(false);
        checkServerStatusButton.gameObject.SetActive(true);
    }
}
