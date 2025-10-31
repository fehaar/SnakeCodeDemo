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
    private InputActionReference movement;

    private SnakeView snakeView;

    void Start()
    {
        snakeView = Instantiate(snakePrefab);
        snakeView.Initialize(new Snake(snakeStartingSpeed));

        movement.ToInputAction().performed += PlayerMovement_Performed;
    }

    private void PlayerMovement_Performed(InputAction.CallbackContext context)
    {
        var moveValue = context.ReadValue<Vector2>();
        if (moveValue.x != 0) 
        {
            snakeView.Turn(moveValue.x > 0 ? Snake.MoveDirection.Right : Snake.MoveDirection.Left);
        }
        else 
        {
            snakeView.Turn(moveValue.y > 0 ? Snake.MoveDirection.Up : Snake.MoveDirection.Down);
        }
    }
}
