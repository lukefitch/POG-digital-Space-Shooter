using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text livesText;
    [SerializeField]
    private TMP_Text gameOverText;
    [SerializeField]
    private TMP_Text restartText;

    private bool _isGameOver;

    void Start()
    {
        scoreText.text = "Score: 0";
        livesText.text = "Lives: 3";
        gameOverText.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);
        _isGameOver = false;
    }

    void Update()
    {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            // Load the main menu scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Menu");
        }
    }

    public void UpdateScore(int playerScore)
    {
        scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        livesText.text = "Lives: " + currentLives;

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void GameOverSequence()
    {
        _isGameOver = true;
        gameOverText.gameObject.SetActive(true);
        restartText.gameObject.SetActive(true);
    }
}
