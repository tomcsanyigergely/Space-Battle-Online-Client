using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UInt8 = System.Byte;
using UdpMessages.ClientServerMessages;

public partial class GameManager
{
    private bool playerAttackedAudioPlaying = false;
    private bool playerAttackingAudioPlaying = false;
    private bool isPlayerAttacking = false;

    private void UpdateAttackedIcons()
    {
        bool playerOneAttacked = false;
        for (int i = 0; i < controlPoints.Count; i++)
        {
            if (controlPoints[i].GetComponent<ControlPoint>().Owner == 0 &&
                controlPoints[i].GetComponent<ControlPoint>().IsContesting[1])
            {
                playerOneAttacked = true;
                break;
            }
        }

        bool playerTwoAttacked = false;
        for (int i = 0; i < controlPoints.Count; i++)
        {
            if (controlPoints[i].GetComponent<ControlPoint>().Owner == 1 &&
                controlPoints[i].GetComponent<ControlPoint>().IsContesting[0])
            {
                playerTwoAttacked = true;
                break;
            }
        }

        progressionDatas[0].GetComponent<AttackedDrawer>().SetAttacked(playerOneAttacked);
        progressionDatas[1].GetComponent<AttackedDrawer>().SetAttacked(playerTwoAttacked);

        if (progressionDatas[playerNumber / teamSize].GetComponent<AttackedDrawer>().Attacked)
        {
            if (!playerAttackedAudioPlaying)
            {
                playerAttackedAudioPlaying = true;
                StartCoroutine(PlayerAttackedAudioCoroutine());
            }
        }
    }

    private IEnumerator PlayerAttackedAudioCoroutine()
    {
        do
        {
            playerAttackedAudio.Play();
            float captureProgression = 0;
            for (int i = 0; i < controlPoints.Count; i++)
            {
                ControlPoint controlPointComponent = controlPoints[i].GetComponent<ControlPoint>();
                if (controlPointComponent.Owner.HasValue &&
                    controlPointComponent.Owner.Value == playerNumber / teamSize &&
                    controlPointComponent.IsContesting[1 - controlPointComponent.Owner.Value] &&
                    controlPointComponent.ProgressedPlayer == 1 - controlPointComponent.Owner.Value)
                {
                    captureProgression = Mathf.Max(captureProgression, controlPointComponent.Progression / controlPointTakingProgression);
                }
            }
            yield return new WaitForSeconds(1.0f - 0.8f * captureProgression);
        } while (progressionDatas[playerNumber / teamSize].GetComponent<AttackedDrawer>().Attacked && clientState == ClientState.Running);
        playerAttackedAudioPlaying = false;
    }

    private int[] numberOfControlledPoints = { 0, 0 };

    private void UpdateControlledPoints()
    {
        UInt8 playerOneControlledPoints = 0;
        UInt8 playerTwoControlledPoints = 0;

        for (int i = 0; i < controlPoints.Count; i++)
        {
            if (controlPoints[i].GetComponent<ControlPoint>().Owner == 0)
            {
                playerOneControlledPoints++;
            }
            else if (controlPoints[i].GetComponent<ControlPoint>().Owner == 1)
            {
                playerTwoControlledPoints++;
            }
        }

        controlledPointsTexts[0].text = Convert.ToString(playerOneControlledPoints);
        controlledPointsTexts[1].text = Convert.ToString(playerTwoControlledPoints);

        if (playerOneControlledPoints > playerTwoControlledPoints)
        {
            controlledPointsBackgrounds[0].GetComponent<Image>().color = playerColors[0];
            controlledPointsBackgrounds[1].GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
            controlledPointsTexts[0].color = Color.black;
            controlledPointsTexts[1].color = playerColors[1];
        }
        else if (playerTwoControlledPoints > playerOneControlledPoints)
        {
            controlledPointsBackgrounds[0].GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
            controlledPointsBackgrounds[1].GetComponent<Image>().color = playerColors[1];
            controlledPointsTexts[0].color = playerColors[0];
            controlledPointsTexts[1].color = Color.black;
        }
        else
        {
            controlledPointsBackgrounds[0].GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
            controlledPointsBackgrounds[1].GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
            controlledPointsTexts[0].color = playerColors[0];
            controlledPointsTexts[1].color = playerColors[1];
        }

        numberOfControlledPoints[0] = playerOneControlledPoints;
        numberOfControlledPoints[1] = playerTwoControlledPoints;
    }

    private IEnumerator Countdown(int countdown)
    {
        while (countdown > 0 && clientState == ClientState.Countdown)
        {
            countdownAudio.Play();
            messageText.text = Convert.ToString(countdown);
            yield return new WaitForSeconds(1.0f);
            countdown--;
        }
    }

    private IEnumerator RespawnCountdown(int countdown)
    {
        while (countdown > 0 && clientState == ClientState.Running)
        {
            messageText.text = Convert.ToString("Respawn in\n" + countdown + "\nseconds");
            yield return new WaitForSeconds(1.0f);
            countdown--;
        }
        if (clientState == ClientState.Running)
        {
            messageText.text = "";                
        }
    }

    private void UpdatePlayerAttackingAudio()
    {
        for (int i = 0; i < controlPoints.Count; i++)
        {
            if (controlPoints[i].GetComponent<ControlPoint>().Owner != playerNumber / teamSize &&
                controlPoints[i].GetComponent<ControlPoint>().IsContesting[playerNumber / teamSize])
            {
                isPlayerAttacking = true;
                if (!playerAttackingAudioPlaying)
                {
                    playerAttackingAudioPlaying = true;
                    StartCoroutine(PlayerAttackingAudioCoroutine());
                }
                return;
            }
        }
        isPlayerAttacking = false;
    }

    private IEnumerator PlayerAttackingAudioCoroutine()
    {
        do
        {
            playerAttackingAudio.Play();
            yield return new WaitForSeconds(1.0f);
        }
        while (isPlayerAttacking && clientState == ClientState.Running);
        playerAttackingAudioPlaying = false;
    }

    private enum WarningState {
        NoWarning, WarningNoAttack, WarningAttack
    }

    private WarningState warningState = WarningState.NoWarning;
    private float timeToChangeWarning;

    private void CheckWarningState()
    {
        WarningState newWarningState;

        if (progressionDatas[1 - (playerNumber / teamSize)].GetComponent<ReinstallTime>().RemainingTime <= 10 &&
            enemyControlProgression.GetComponent<ControlProgression>().controlProgression >= 80 &&
            numberOfControlledPoints[1-(playerNumber / teamSize)] > numberOfControlledPoints[playerNumber / teamSize])
        {
            if (progressionDatas[1 - (playerNumber / teamSize)].GetComponent<AttackedDrawer>().Attacked)
            {
                newWarningState = WarningState.WarningAttack;
            }
            else
            {
                newWarningState = WarningState.WarningNoAttack;
            }
        }
        else
        {
            newWarningState = WarningState.NoWarning;
        }

        if (newWarningState != warningState)
        {
            TransitionToWarningState(newWarningState);            
        }
        else
        {
            switch (warningState)
            {
                case WarningState.WarningNoAttack:
                    ChangeWarning();
                    break;
                default:
                    break;
            }
        }
    }

    private void TransitionToWarningState(WarningState newWarningState)
    {
        this.warningState = newWarningState;

        switch (newWarningState)
        {
            case WarningState.NoWarning:
                enemyNumberBackgroundHighlighted.gameObject.SetActive(false);
                enemyProgressionHighlighted.gameObject.SetActive(false);
                break;
            case WarningState.WarningNoAttack:
                timeToChangeWarning = 0;
                ChangeWarning();
                break;
            case WarningState.WarningAttack:
                enemyNumberBackgroundHighlighted.gameObject.SetActive(true);
                enemyProgressionHighlighted.gameObject.SetActive(true);
                break;
        }
    }

    private void ChangeWarning()
    {
        if (timeToChangeWarning <= 0)
        {
            enemyNumberBackgroundHighlighted.gameObject.SetActive(!enemyNumberBackgroundHighlighted.gameObject.activeSelf);
            enemyProgressionHighlighted.gameObject.SetActive(!enemyProgressionHighlighted.gameObject.activeSelf);

            if (enemyProgressionHighlighted.gameObject.activeSelf)
            {
                warningAudio.Play();
            }

            timeToChangeWarning += 0.5f;
        }
        timeToChangeWarning -= Time.deltaTime;
    }    

    private void UpdateTimeToDraw()
    {
        if (numberOfControlledPoints[playerNumber / teamSize] > numberOfControlledPoints[1 - playerNumber / teamSize] &&
            playerControlProgression.GetComponent<ControlProgression>().controlProgression == maxControlProgression ||
            numberOfControlledPoints[1 - playerNumber / teamSize] > numberOfControlledPoints[playerNumber / teamSize] &&
            enemyControlProgression.GetComponent<ControlProgression>().controlProgression == maxControlProgression ||
            numberOfControlledPoints[playerNumber / teamSize] == numberOfControlledPoints[1 - playerNumber / teamSize])
        {
            timeToDraw -= Time.deltaTime;
            timeToDrawText.text = TimeSpan.FromSeconds(Mathf.CeilToInt(timeToDraw)).ToString(@"mm\:ss");
            timeToDrawText.color = Color.white;
        }
        else
        {
            timeToDrawText.color = new Color(100.0f / 255.0f, 100.0f / 255.0f, 100.0f / 255.0f);
        }
    }
}
