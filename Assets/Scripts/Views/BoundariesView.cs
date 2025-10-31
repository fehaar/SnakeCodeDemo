using UnityEngine;

public class BoundariesView : MonoBehaviour
{
    private Snake snake;
    private Boundaries boundaries;

    public void Initialize(Boundaries boundaries, Snake snake)
    {
        this.snake = snake;
        this.boundaries = boundaries;
        transform.localScale = boundaries.Size;
    }

    private void Update()
    {
        // Check if the snake is inside the boundaries, otherwise kill it
        if (!boundaries.IsInside(snake.Position))
        {
            
        }
    }
}
