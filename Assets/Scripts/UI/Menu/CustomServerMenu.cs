using System;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomServerMenu : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private NetworkHandler networkHandler;
    
    [SerializeField] private Canvas customServerMenu;
    
    [SerializeField] private TextMeshProUGUI customServerText;
    [SerializeField] private TextMeshProUGUI establishingConnectionText;
    [SerializeField] private TextMeshProUGUI connectionFailedText;
    [SerializeField] private TextMeshProUGUI versionText;

    [SerializeField] private TMP_InputField ipAddressInputField;
    [SerializeField] private TMP_InputField portInputField;

    [SerializeField] private Button connectButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button leaveButton;
    [SerializeField] private Button connectionFailedOkButton;

    private bool connecting;
    
    private void Start()
    {
        versionText.text = "v" + Config.MAJOR_VERSION + "." + Config.MINOR_VERSION + "." + Config.PATCH_VERSION;
        
        connectButton.onClick.AddListener(() =>
        {
            try
            {
                bool validIp = IPAddress.TryParse(ipAddressInputField.text, out IPAddress ipAddress);
                if (validIp)
                {
                    UInt16 port = Convert.ToUInt16(portInputField.text);
                    Connect(ipAddressInputField.text, port);
                }
                else
                {
                    gameSystem.CustomServerMenu();
                }
            }
            catch (Exception e)
            {
                gameSystem.CustomServerMenu();
            }
        });
        
        connectionFailedOkButton.onClick.AddListener(() => {
            gameSystem.CustomServerMenu();
        });
        
        cancelButton.onClick.AddListener(() => {
            CancelConnection();
        });
        
        leaveButton.onClick.AddListener(() => gameSystem.SelectServerMenu());

        UIUtilities.Hide(customServerMenu);
    }

    public void Enter()
    {
        connecting = false;

        ipAddressInputField.text = "";
        portInputField.text = "";
        
        ipAddressInputField.gameObject.SetActive(true);
        portInputField.gameObject.SetActive(true);
        
        customServerText.gameObject.SetActive(true);
        establishingConnectionText.gameObject.SetActive(false);
        connectionFailedText.gameObject.SetActive(false);

        connectButton.gameObject.SetActive(true);
        connectionFailedOkButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);

        leaveButton.gameObject.SetActive(true);

        UIUtilities.Show(customServerMenu);
    }

    public void Exit()
    {
        UIUtilities.Hide(customServerMenu);
    }
    
    private void Connect(String ip, UInt16 port)
    {
        ipAddressInputField.gameObject.SetActive(false);
        portInputField.gameObject.SetActive(false);
        
        customServerText.gameObject.SetActive(false);
        establishingConnectionText.gameObject.SetActive(true);
        connectButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(true);

        leaveButton.gameObject.SetActive(false);

        connecting = true;

        networkHandler.Connect(ip, port);
    }

    private void CancelConnection()
    {
        if (connecting)
        {
            connecting = false;
            networkHandler.Disconnect();
            gameSystem.CustomServerMenu();
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
    }
}
