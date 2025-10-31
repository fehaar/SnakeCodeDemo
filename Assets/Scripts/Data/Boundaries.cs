using UnityEngine;

/// <summary>
/// The boundaries define the size of the play area and can kill a snake if it goes outside the area
/// </summary>
public class Boundaries
{
    public Boundaries(Bounds bounds)
    {
        this.bounds = bounds; 
    }

    private Bounds bounds;

    internal Vector3 Size => bounds.size;
    internal Vector2 Min => bounds.min;
    internal Vector2 Max => bounds.max;

    internal void KillSnakeOutsideArea(Snake snake)
    {
        if (!bounds.Contains(snake.Position))
        {
            snake.Kill();
        }
    }
}
