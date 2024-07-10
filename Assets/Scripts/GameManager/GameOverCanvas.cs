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
    
    private void OnEnable()
    {
        timeSurvivedText.text = $"Time survived: {60 - GameManager.instance.timeTimer}";
        killsCounterText.text = $"Enemies killed: {GameManager.instance.killCounter}";
    }

    public void OnBackToLobbyButton()
    { 
        gameOverCanvas.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
