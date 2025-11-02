using System;
using Unity.Collections;
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

        // Initialize the snake line segments
        lineSegments1[0] = Position;
        lineSegments1[1] = Position;
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

    public event Action OnDead;

    private float snakeLengthIncrement = 20f;
    private float snakeSpeedIncrement = 1f;

    private int startIndex = 0;
    private int length = 2;
    private NativeArray<Vector3> lineSegments1 = new NativeArray<Vector3>(2000, Allocator.Domain);

    /// <summary>
    /// Which way is the snake moving?
    /// </summary>
    public MoveDirection CurrentDirection { get; set; } = MoveDirection.Right;

    public NativeSlice<Vector3> Positions => lineSegments1.Slice(startIndex, length);

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
        lineSegments1[length - 1] = Position;
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
        length++;
    }

    internal void Kill()
    {
        IsDead = true;
        OnDead?.Invoke();
    }

    internal void Eat(FoodView food)
    {
        Length += snakeLengthIncrement;
        Speed += snakeSpeedIncrement;
    }
}
