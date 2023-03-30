using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        
    }

    public void GameOver()
    {
        // Pause the game
        Time.timeScale = 0f;

        // Show the game over screen
        UIManager uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        uiManager.GameOverSequence();
    }
}
