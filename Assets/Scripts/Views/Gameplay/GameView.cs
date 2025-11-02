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

    private Snake snake;

    private SnakeView snakeView;

    internal void StartGame()
    {
        // Set up the game
        snake = snakeSettings.CreateSnake();
        snakeView = Instantiate(snakePrefab);
        snakeView.Initialize(snake, movement.ToInputAction());

        var gameArea = gameAreaSettings.CreateGameArea();
        // Find this with code instead of having a firm reference as we would currently not benefit from having more than one in a scene
        var gameAreaView = FindFirstObjectByType<GameAreaView>();
        gameAreaView.Initialize(gameArea, snake);

        var foodArea = foodAreaSettings.CreateFoodArea(gameArea);
        // Find this with code instead of having a firm reference as we would currently not benefit from having more than one in a scene
        var foodAreaView = FindFirstObjectByType<FoodAreaView>();
        foodAreaView.Initialize(foodArea, snake);

        scoreView.Initialize(foodArea);
    }
}
