using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UdpMessages.ServerClientMessages;
using UdpMessages.ClientServerMessages;

public class VersionScreen : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private NetworkHandler networkHandler;

    [SerializeField] private Canvas versionScreen;

    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI versionText;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button disconnectButton;

    private bool gettingVersion;
    private bool disconnecting;
    private float timeToGetVersion;

    private void Start()
    {
        versionText.text = "v" + Config.MAJOR_VERSION + "." + Config.MINOR_VERSION + "." + Config.PATCH_VERSION;

        continueButton.onClick.AddListener(() => {
            gameSystem.MainMenu();
        });

        disconnectButton.onClick.AddListener(() => {
            disconnecting = true;
            continueButton.interactable = false;
            networkHandler.Disconnect();
        });

        UIUtilities.Hide(versionScreen);
    }

    public void Enter()
    {
        disconnecting = false;
        gettingVersion = true;
        timeToGetVersion = 0.5f;

        continueButton.interactable = true;

        continueButton.gameObject.SetActive(false);
        disconnectButton.gameObject.SetActive(false);

        messageText.text = "Checking compatibility...";

        UIUtilities.Show(versionScreen);

        GetVersion();
    }

    // Called each frame from GameSystem
    public void MenuUpdate()
    {
        if (gettingVersion)
        {
            timeToGetVersion -= Time.deltaTime;
            if (timeToGetVersion <= 0)
            {
                GetVersion();
                do
                {
                    timeToGetVersion += 0.5f;
                } while (timeToGetVersion <= 0);
            }
        }
    }

    private void GetVersion()
    {
        GetVersionMessage.StartGetVersionMessage(networkHandler.builder);
        var message = GetVersionMessage.EndGetVersionMessage(networkHandler.builder);
        networkHandler.Send(message);
    }

    public void Exit()
    {
        UIUtilities.Hide(versionScreen);
    }

    public void OnReceived(VersionMessage versionMessage)
    {
        gettingVersion = false;

        var minVersion = versionMessage.Fix.Value.MinVersion;
        var maxVersion = versionMessage.Fix.Value.MaxVersion;

        bool oneVersionSupported = (minVersion.MajorVersion == maxVersion.MajorVersion) && (minVersion.MinorVersion == maxVersion.MinorVersion) && (minVersion.PatchVersion == maxVersion.PatchVersion);

        if (Config.MAJOR_VERSION < minVersion.MajorVersion ||
            Config.MAJOR_VERSION == minVersion.MajorVersion && Config.MINOR_VERSION < minVersion.MinorVersion ||
            Config.MAJOR_VERSION == minVersion.MajorVersion && Config.MINOR_VERSION == minVersion.MinorVersion && Config.PATCH_VERSION < minVersion.PatchVersion)
        {
            if (oneVersionSupported)
            {
                messageText.text = "Your client version is not supported.\nUpgrade to v" + minVersion.MajorVersion + "." + minVersion.MinorVersion + "." + minVersion.PatchVersion + ".";
                disconnectButton.gameObject.SetActive(true);
            }
            else
            {
                messageText.text = "Your client version is not supported.\nUpgrade to a newer version between\nv" + minVersion.MajorVersion + "." + minVersion.MinorVersion + "." + minVersion.PatchVersion + " and v" + maxVersion.MajorVersion + "." + maxVersion.MinorVersion + "." + maxVersion.PatchVersion + ".";
                disconnectButton.gameObject.SetActive(true);
            }
        }
        else if (Config.MAJOR_VERSION > maxVersion.MajorVersion ||
                 Config.MAJOR_VERSION == maxVersion.MajorVersion && Config.MINOR_VERSION > maxVersion.MinorVersion ||
                 Config.MAJOR_VERSION == maxVersion.MajorVersion && Config.MINOR_VERSION == maxVersion.MinorVersion && Config.PATCH_VERSION > maxVersion.PatchVersion)
        {
            if (oneVersionSupported)
            {
                messageText.text = "Your client version is not supported.\nDowngrade to v" + minVersion.MajorVersion + "." + minVersion.MinorVersion + "." + minVersion.PatchVersion + ".";
                disconnectButton.gameObject.SetActive(true);
            }
            else
            {
                messageText.text = "Your client version is not supported.\nDowngrade to an older version between\nv" + minVersion.MajorVersion + "." + minVersion.MinorVersion + "." + minVersion.PatchVersion + " and v" + maxVersion.MajorVersion + "." + maxVersion.MinorVersion + "." + maxVersion.PatchVersion + ".";
                disconnectButton.gameObject.SetActive(true);
            }
        }
        else
        {
            if (Config.MAJOR_VERSION == maxVersion.MajorVersion && Config.MINOR_VERSION == maxVersion.MinorVersion && Config.PATCH_VERSION == maxVersion.PatchVersion)
            {
                gameSystem.MainMenu();
            }
            else
            {
                messageText.text = "A newer version is available: v" + maxVersion.MajorVersion + "." + maxVersion.MinorVersion + "." + maxVersion.PatchVersion + ".";
                continueButton.gameObject.SetActive(true);
                disconnectButton.gameObject.SetActive(true);
            }
        }
    }

    public void OnDisconnected()
    {
        if (disconnecting)
        {
            gameSystem.SelectServerMenu();
        }
        else
        {
            gameSystem.LostConnectionScreen();
        }
    }
}
