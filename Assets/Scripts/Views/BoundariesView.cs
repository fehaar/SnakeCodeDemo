using UnityEngine;

/// <summary>
/// Show the boundaries of the play area
/// </summary>
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
        boundaries.KillSnakeOutsideArea(snake);
    }
}
