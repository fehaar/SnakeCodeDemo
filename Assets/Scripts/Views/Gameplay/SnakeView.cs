using Cysharp.Threading.Tasks;
using PrimeTween;
using System;
using UnityEngine;

/// <summary>
/// This is the visual representation of a Snake in the game.
/// </summary>
public class SnakeView : MonoBehaviour
{
    [SerializeField]
    private TweenSettings<float> deathAnimation;

    private Snake snake;
    private LineRenderer line;
    private Rigidbody2D body;

    const float COLLISION_DISTANCE = 0.5f;

    // Array caches for positioning so we don't always have to make new ones
    private Vector3[] oldPositions = Array.Empty<Vector3>();
    private Vector3[] newPositions = Array.Empty<Vector3>();

    public void Initialize(GameData gameData, UnityEngine.InputSystem.InputAction inputAction)
    {
        this.snake = gameData.Snake;
        gameData.AddEndGameTaskFactory(EndGameAnimation);
        this.line = GetComponent<LineRenderer>();
        inputAction.performed += InputAction_performed;
        this.body = GetComponent<Rigidbody2D>();
    }

    private void InputAction_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        var moveValue = context.ReadValue<Vector2>();
        if (moveValue.x != 0)
        {
            Turn(moveValue.x > 0 ? Snake.MoveDirection.Right : Snake.MoveDirection.Left);
        }
        else
        {
            Turn(moveValue.y > 0 ? Snake.MoveDirection.Up : Snake.MoveDirection.Down);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (snake.IsDead)
        {
            return;
        }

        // Tick the snake's logic
        snake.Tick(Time.deltaTime);

        // Update the visual representation
        SetSnakePositions();
        body.MovePosition(snake.Position);

        // Check if we are hitting out own line
        CheckCollisionWithSelf();
    }

    private void SetSnakePositions()
    {
        var positions = snake.Positions;
        line.positionCount = positions.Length;
        line.SetPositions(positions);
    }

    private void CheckCollisionWithSelf()
    {
        // As we are moving perpendicularly, it is impossible to collide with the two line segmensts closest to the head
        if (line.positionCount > 3)
        {
            for (int i = 0; i < line.positionCount - 3; i++)
            {
                var first = line.GetPosition(i);
                var second = line.GetPosition(i + 1);
                if (first.x == second.x)
                {
                    // This is a vertical line - is our head x close enough to the line
                    if (Math.Abs(snake.Position.x - first.x) <= COLLISION_DISTANCE)
                    {
                        if (snake.Position.y < Math.Max(first.y, second.y) && snake.Position.y > Math.Min(first.y, second.y))
                        {
                            snake.Kill();
                        }
                    }
                }
                else
                {
                    // This is a horizontal line - is our head y close enough to the line
                    if (Math.Abs(snake.Position.y - first.y) <= COLLISION_DISTANCE)
                    {
                        if (snake.Position.x < Math.Max(first.x, second.x) && snake.Position.x > Math.Min(first.x, second.x))
                        {
                            snake.Kill();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Since the history of the snake is only held in the line renderer, we will have to do correction of the snake directly in the points of the renderer
    /// </summary>
    private void ReduceTailLength(float currentLength)
    {
        snake.AdjustTailLength(currentLength);
        SetSnakePositions();
    }

    private static float GetSegmentLength(Vector3 first, Vector3 second)
    {
        // Since the lines are always perpendicular - we can avoid using sqrt
        if (first.x == second.x)
        {
            return Mathf.Abs(first.y - second.y);
        }
        else
        {
            return Mathf.Abs(first.x - second.x);
        }
    }

    private async UniTask EndGameAnimation()
    {
        deathAnimation.startValue = snake.Length;
        await Tween.Custom(deathAnimation, ReduceTailLength);
        Destroy(gameObject);
        snake.Dispose();
    }

    private void Turn(Snake.MoveDirection moveDirection)
    {
        // If we are dead, we will not turn the snake
        if (snake.IsDead)
        {
            return;
        }
        // We will add in a new segment of the line when we turn, and initialize it to the head of the snake.
        snake.Turn(moveDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var food = collision.gameObject.GetComponentInParent<FoodView>();
        food.Eat();
        snake.Eat(food);
    }
}
