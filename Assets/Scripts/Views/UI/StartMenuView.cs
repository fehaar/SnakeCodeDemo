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
    private List<SnakeSettings> snakeSettings = new List<SnakeSettings>();
    [SerializeField]
    private TMP_Dropdown snakeSettingsSelector;

    [SerializeField]
    private List<GameAreaSettings> gameAreaSettings = new List<GameAreaSettings>();
    [SerializeField]
    private TMP_Dropdown gameAreaSettingsSelector;

    [SerializeField]
    private List<FoodAreaSettings> foodAreaSettings = new List<FoodAreaSettings>();
    [SerializeField]
    private TMP_Dropdown foodAreaSettingsSelector;

    void Start()
    {
        snakeSettingsSelector.AddOptions(snakeSettings.Select(s => s.name).ToList());
        gameAreaSettingsSelector.AddOptions(gameAreaSettings.Select(s => s.name).ToList());
        foodAreaSettingsSelector.AddOptions(foodAreaSettings.Select(s => s.name).ToList());

        startButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        gameObject.SetActive(false);

        var gameView = FindFirstObjectByType<GameView>();
        gameView.StartGame(new GameData(snakeSettings[snakeSettingsSelector.value], gameAreaSettings[gameAreaSettingsSelector.value], foodAreaSettings[foodAreaSettingsSelector.value]));
    }
}
