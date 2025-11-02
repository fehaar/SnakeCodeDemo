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

    internal void StartGame(GameData gameData)
    {
        this.gameData = gameData;

        // Set up the snake
        snakeView = Instantiate(snakePrefab);
        snakeView.Initialize(gameData, movement.ToInputAction());
        gameData.Snake.OnDead += Snake_OnDead;

        // Initialize all the other parts of the game in way where we do not need explicit references
        foreach (var initializer in FindObjectsByType<GameDataInitializable>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            initializer.Initialize(gameData);
        }
    }

    private void Snake_OnDead()
    {
        gameData.Snake.OnDead -= Snake_OnDead;
        gameData.EndGame();
        gameData = null;
    }
}
