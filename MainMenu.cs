using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;

    private void Start()
    {
        // Assign the LoadGame function to the startButton's onClick event
        startButton.onClick.AddListener(LoadGame);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
