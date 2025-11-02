using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is a simple start game menu allowing you to begin playing
/// </summary>
public class StartMenuView : MonoBehaviour
{
    [SerializeField]
    private Button startButton;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        gameObject.SetActive(false);

        var gameView = FindFirstObjectByType<GameView>();
        gameView.StartGame();
    }
}
