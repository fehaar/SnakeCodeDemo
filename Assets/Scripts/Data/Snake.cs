using UnityEngine;

/// <summary>
/// This is the data representation of a Snake in the game.
/// The snake only knows of its head and how to move that.
/// </summary>
public class Snake
{
    public Snake(float snakeStartingSpeed, int snakeStartingLength, float snakeLengthIncrement, float snakeSpeedIncrement)
    {
        this.Speed = snakeStartingSpeed;
        Length = snakeStartingLength;
        this.snakeLengthIncrement = snakeLengthIncrement;
        this.snakeSpeedIncrement = snakeSpeedIncrement;
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

    public bool IsDead { get; private set; } = false;

    private float snakeLengthIncrement = 20f;
    private float snakeSpeedIncrement = 1f;

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
        // Make sure that we don't move backwards
        switch (CurrentDirection)
        {
            case MoveDirection.Up:
                if (moveDirection == MoveDirection.Down)
                {
                    return;
                }
                break;
            case MoveDirection.Down:
                if (moveDirection == MoveDirection.Up) 
                { 
                    return; 
                }
                break;
            case MoveDirection.Left:
                if (moveDirection == MoveDirection.Right)
                {
                    return;
                }
                break;
            case MoveDirection.Right:
                if (moveDirection == MoveDirection.Left)
                {
                    return;
                }
                break;
            default:
                break;
        }

        CurrentDirection = moveDirection;
    }

    internal void Kill()
    {
        IsDead = true;
    }

    internal void Eat(FoodView food)
    {
        Length += snakeLengthIncrement;
        Speed += snakeSpeedIncrement;
    }
}
