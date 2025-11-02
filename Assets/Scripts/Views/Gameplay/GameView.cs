using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class represents the main game view in the application.
/// It's responsibility is to pull all of the other views together and manage the overall game state.
/// </summary>
public class GameView : MonoBehaviour
{
    [Header("Snake")]
    [SerializeField]
    private SnakeView snakePrefab;
    [SerializeField]
    private SnakeSettings snakeSettings;
    [SerializeField]
    private InputActionReference movement;

    [Header("Play area")]
    [SerializeField]
    private GameAreaSettings gameAreaSettings;

    [Header("Food")]
    [SerializeField]
    private FoodAreaSettings foodAreaSettings;

    [Header("Score")]
    [SerializeField]
    private ScoreView scoreView;

    private GameData gameData;

    private SnakeView snakeView;

    internal void StartGame()
    {
        gameData = new GameData(snakeSettings, gameAreaSettings, foodAreaSettings);

        // Set up the game
        snakeView = Instantiate(snakePrefab);
        snakeView.Initialize(gameData.Snake, movement.ToInputAction());

        // Find this with code instead of having a firm reference as we would currently not benefit from having more than one in a scene
        var gameAreaView = FindFirstObjectByType<GameAreaView>();
        gameAreaView.Initialize(gameData);

        // Find this with code instead of having a firm reference as we would currently not benefit from having more than one in a scene
        var foodAreaView = FindFirstObjectByType<FoodAreaView>();
        foodAreaView.Initialize(gameData);

        scoreView.Initialize(gameData);
    }
}
