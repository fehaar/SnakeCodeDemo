using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class represents the main game view in the application.
/// It's responsibility is to pull all of the other views together and manage the overall game state.
/// </summary>
public class GameView : MonoBehaviour
{
    [SerializeField]
    private SnakeView snakePrefab;
    [SerializeField]
    private float snakeStartingSpeed = 1f;

    [SerializeField]
    private BoundariesView boundariesView;
    [SerializeField]
    private Vector2 boundariesStartingSize = new Vector2(150, 100);

    [SerializeField]
    private InputActionReference movement;

    private Snake snake;

    private SnakeView snakeView;

    void Start()
    {
        snake = new Snake(snakeStartingSpeed);

        snakeView = Instantiate(snakePrefab);
        snakeView.Initialize(snake, movement.ToInputAction());

        boundariesView.Initialize(new Boundaries(new Bounds(Vector3.zero, boundariesStartingSize)), snake);
    }
}
