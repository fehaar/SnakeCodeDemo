using UnityEngine;

/// <summary>
/// This is the data representation of a Snake in the game.
/// </summary>
public class Snake
{
    public Snake(float snakeStartingSpeed)
    {
        this.Speed = snakeStartingSpeed;
    }

    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    /// <summary>
    /// Where is the head of the snake located?
    /// </summary>
    public Vector2 Position { get; private set; }

    /// <summary>
    /// How fast is the snake moving
    /// </summary>
    public float Speed { get; private set; } = 1f;

    /// <summary>
    /// How long is the snake
    /// </summary>
    public float Length { get; private set; } = 20f;

    /// <summary>
    /// Which way is the snake moving?
    /// </summary>
    public MoveDirection CurrentDirection { get; set; } = MoveDirection.Right;

    internal void Tick(float deltaTime)
    {
        var distance = Speed * deltaTime;
        switch (CurrentDirection)
        {
            case MoveDirection.Up:
                Position += new Vector2(0, distance);
                break;
            case MoveDirection.Down:
                Position += new Vector2(0, -distance);
                break;
            case MoveDirection.Left:
                Position += new Vector2(-distance, 0);
                break;
            case MoveDirection.Right:
                Position += new Vector2(distance, 0);
                break;
        }
    }

    internal void Turn(MoveDirection moveDirection)
    {
        CurrentDirection = moveDirection;
    }
}
