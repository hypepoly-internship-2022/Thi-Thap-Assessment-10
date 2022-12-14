using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] PlayerController playerController;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] float timeLeft;
    [SerializeField] GameObject goalGameObject;
    public bool timerOn = true;

    public enum GameState
    {
        Play,
        Win,
        Loose,
    }
    GameState gameState;

    private void Awake()
    {
        gameState = GameState.Play;
        timerOn = true;

    }
    void Start()
    {
        
    }

    
    void Update()
    {
        switch (gameState)
        {
            case GameState.Play:
                GameManagerGameState();
                break;
            case GameState.Loose:
                GameManagerLooseState();
                break;
            case GameState.Win:
                GameManagerWinState();
                break;
        }
    }
    public void SetGameManagerGameState()
    {
        gameState = GameState.Play;
    }
    public void SetGameManagerLooseState()
    {
        gameState = GameState.Loose;
    }
    public void SetGameManagerWinState()
    {
        gameState = GameState.Win;
    }
    void GameManagerGameState()
    {
        CountDownTimer();
    }
    void CountDownTimer()
    {
        if (timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimer(timeLeft);
            }
            else
            {
                gameState = GameState.Loose;
                timeLeft = 0;
                timerOn = false;
            }
        }
    }
    void UpdateTimer(float currentTime)
    {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
    void GameManagerLooseState()
    {
        timeText.text = "Game Over";
        DeactivePlayerAndEnemy();
        goalGameObject.SetActive(false);
    }
    void GameManagerWinState()
    {
        timeText.text = "You Win";
        DeactivePlayerAndEnemy();
    }
    void DeactivePlayerAndEnemy()
    {
        enemy.gameObject.GetComponent<Enemy>().enabled = false;
        playerController.gameObject.GetComponent<PlayerController>().enabled = false;
    }
}