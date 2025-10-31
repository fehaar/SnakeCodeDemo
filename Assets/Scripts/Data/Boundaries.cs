using UnityEngine;

/// <summary>
/// The boundaries define the size of the play area
/// </summary>
public class Boundaries
{
    public Boundaries(Bounds bounds)
    {
        this.bounds = bounds; 
    }

    private Bounds bounds;

    internal Vector3 Size => bounds.size;

    internal void KillSnakeOutsideArea(Snake snake)
    {
        if (!bounds.Contains(snake.Position))
        {
            snake.Kill();
        }
    }
}
