using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnterUsername : MonoBehaviour
{
    [SerializeField] private GameSystem gameSystem;

    [SerializeField] private Canvas enterUsernameCanvas;

    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button cancelButton;

    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField, TextArea] private string usernameErrorText;

    private void Start()
    {
        continueButton.onClick.AddListener(() => {
            errorText.text = "";

            string username = usernameInputField.text;
            if (username.Length >= 3)
            {
                gameSystem.SelectPlayersMenu(username);
            }
            else
            {
                errorText.text += usernameErrorText;
            }
        });

        cancelButton.onClick.AddListener(() => {
            gameSystem.MainMenu();
        });

        UIUtilities.Hide(enterUsernameCanvas);
    }

    public void Enter()
    {
        usernameInputField.text = "";
        errorText.text = "";

        UIUtilities.Show(enterUsernameCanvas);
    }

    public void Exit()
    {
        UIUtilities.Hide(enterUsernameCanvas);
    }
}
