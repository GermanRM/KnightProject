using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCanvas : MonoBehaviour
{
    [Header("Info Properties")]
    [SerializeField] private TMP_Text timeSurvivedText;
    [SerializeField] private TMP_Text killsCounterText;
    [SerializeField] private GameObject gameOverCanvas;

    [Header("Win Properties")]
    [SerializeField] private TMP_Text winTimeSurvivedText;
    [SerializeField] private TMP_Text winKillsCounterText;
    [SerializeField] private GameObject winCanvas;

    public void OnPlayerDeath()
    {
        timeSurvivedText.text = $"Time survived: {(int) (60 - GameManager.instance.timeTimer)}";
        killsCounterText.text = $"Enemies killed: {GameManager.instance.killCounter}";
    }

    public void OnPlayerWin()
    {
        winKillsCounterText.text = $"Enemies killed: {GameManager.instance.killCounter}";
    }

    public void OnBackToLobbyButton()
    { 
        gameOverCanvas.SetActive(false);
        winCanvas.SetActive(false);
        GameManager.instance.killCounter = 0;
        SceneManager.LoadScene("MainMenu");
        GameManager.instance.finalBossSpawned = false;
    }
}
