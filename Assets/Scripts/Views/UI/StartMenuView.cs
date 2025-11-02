using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is a simple start game menu allowing you to begin playing
/// </summary>
public class StartMenuView : MonoBehaviour
{
    [SerializeField]
    private Button startButton;

    [SerializeField]
    private List<SnakeSettings> snakes = new List<SnakeSettings>();
    [SerializeField]
    private TMP_Dropdown snakeSettingsSelector;

    [SerializeField]
    private List<GameAreaSettings> gameAreaSettings = new List<GameAreaSettings>();

    [SerializeField]
    private List<FoodAreaSettings> foodAreaSettings = new List<FoodAreaSettings>();

    void Start()
    {
        snakeSettingsSelector.AddOptions(snakes.Select(s => s.name).ToList());

        startButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        gameObject.SetActive(false);

        var gameView = FindFirstObjectByType<GameView>();
        gameView.StartGame(new GameData(snakes[snakeSettingsSelector.value], gameAreaSettings[0], foodAreaSettings[0]));
    }
}
