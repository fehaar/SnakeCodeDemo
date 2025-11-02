using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This represents the main game view in the application.
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

    private GameData gameData;

    private SnakeView snakeView;

    internal void StartGame()
    {
        gameData = new GameData(snakeSettings, gameAreaSettings, foodAreaSettings);

        // Set up the snake
        snakeView = Instantiate(snakePrefab);
        snakeView.Initialize(gameData.Snake, movement.ToInputAction());

        // Initialize all the other parts of the game in way where we do not need explicit references
        foreach (var initializer in FindObjectsByType<GameDataInitializable>(FindObjectsSortMode.None))
        {
            initializer.Initialize(gameData);
        }
    }
}
