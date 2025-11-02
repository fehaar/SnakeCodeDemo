using UnityEngine;

/// <summary>
/// Show the boundaries of the play area
/// </summary>
public class GameAreaView : MonoBehaviour
{
    private Snake snake;
    private GameArea boundaries;

    public void Initialize(GameArea boundaries, Snake snake)
    {
        this.snake = snake;
        this.boundaries = boundaries;
        transform.localScale = boundaries.Size;
    }

    private void Update()
    {
        if (this.boundaries != null)
        {
            boundaries.KillSnakeOutsideArea(snake);
        }
    }
}
