using UnityEngine;

/// <summary>
/// This is the visual representation of a Snake in the game.
/// </summary>
public class SnakeView : MonoBehaviour
{
    private Snake snake;
    private LineRenderer line;

    public void Initialize(Snake snake)
    {
        this.snake = snake;
        this.line = GetComponent<LineRenderer>();
    }

    void Start()
    {
        line.SetPosition(0, new Vector3(snake.Position.x, snake.Position.y, 0));
        line.SetPosition(1, new Vector3(snake.Position.x, snake.Position.y, 0));
    }

    // Update is called once per frame
    void Update()
    {
        // Tick the snake's logic
        snake.Tick(Time.deltaTime);

        // Update the visual representation
        line.SetPosition(line.positionCount - 1, new Vector3(snake.Position.x, snake.Position.y, 0));
    }
}
