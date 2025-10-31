using UnityEngine;

public class Boundaries
{
    public Boundaries(Bounds bounds)
    {
        this.bounds = bounds; 
    }

    private Bounds bounds;

    internal Vector3 Size => bounds.size;

    internal bool IsInside(Vector2 position)
    {
        return bounds.Contains(position);
    }
}
