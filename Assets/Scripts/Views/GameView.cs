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
    private FoodAreaView foodAreaView;
    [SerializeField]
    private float foodSpawnInterval = 5f;
    [SerializeField]
    private int maxFood = 6;

    [Header("Score")]
    [SerializeField]
    private ScoreView scoreView;

    private Snake snake;

    private SnakeView snakeView;

    void Start()
    {
        // Set up the game
        snake = snakeSettings.CreateSnake();
        snakeView = Instantiate(snakePrefab);
        snakeView.Initialize(snake, movement.ToInputAction());

        var gameArea = gameAreaSettings.CreateGameArea();
        var gameAreaView = FindFirstObjectByType<GameAreaView>();
        gameAreaView.Initialize(gameArea, snake);

        var foodArea = new FoodArea(foodSpawnInterval, maxFood, gameArea);
        foodAreaView.Initialize(foodArea, snake);

        scoreView.Initialize(foodArea);
    }
}
