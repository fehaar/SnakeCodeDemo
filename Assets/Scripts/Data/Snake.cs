using System;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// This is the data representation of a Snake in the game.
/// The snake only knows of its head and how to move that.
/// </summary>
public class Snake : IDisposable
{
    public Snake(float snakeStartingSpeed, int snakeStartingLength, float snakeLengthIncrement, float snakeSpeedIncrement)
    {
        this.Speed = snakeStartingSpeed;
        Length = snakeStartingLength;
        this.snakeLengthIncrement = snakeLengthIncrement;
        this.snakeSpeedIncrement = snakeSpeedIncrement;

        // Initialize the snake line segments
        activeLineSegments[0] = Position;
        activeLineSegments[1] = Position;
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
    private int segmentCount = 2;
    private NativeArray<Vector3> activeLineSegments = new NativeArray<Vector3>(2000, Allocator.Domain);

    /// <summary>
    /// Which way is the snake moving?
    /// </summary>
    public MoveDirection CurrentDirection { get; set; } = MoveDirection.Right;

    public NativeSlice<Vector3> Positions => activeLineSegments.Slice(startIndex, segmentCount);

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
        activeLineSegments[startIndex + segmentCount - 1] = Position;
        AdjustTailLength(Length);
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
        // We will add a new segment to the line when we turn
        segmentCount++;
        activeLineSegments[startIndex + segmentCount - 1] = Position;
    }

    internal void AdjustTailLength(float totalLength)
    {
        var lineLength = 0f;
        for (int i = startIndex + segmentCount - 1; i >= startIndex + 1; i--)
        {
            // Sum up the length of the snake from the head and back
            var first = activeLineSegments[i];
            var second = activeLineSegments[i - 1];
            var segmentLength = 0f;
            segmentLength = GetSegmentLength(first, second);
            var lengthDifference = lineLength + segmentLength - totalLength;
            // Stop when we reach the max length of the snake
            if (lengthDifference > 0)
            {
                // We are too long and need to move our tail a bit
                // We start from the back and see how much to cut off
                var segmentsToRemove = 0;
                for (int j = startIndex; j < startIndex + segmentCount - 1; j++)
                {
                    first = activeLineSegments[j];
                    second = activeLineSegments[j + 1];
                    segmentLength = GetSegmentLength(first, second);
                    if (lengthDifference >= segmentLength)
                    {
                        // We need to cull a segment and then proceed with shortening
                        segmentsToRemove++;
                        lengthDifference -= segmentLength;
                    }
                    else
                    {
                        if (first.x == second.x)
                        {
                            if (first.y > second.y)
                            {
                                activeLineSegments[j] = new Vector3(first.x, first.y - lengthDifference, 0);
                            }
                            else
                            {
                                activeLineSegments[j] = new Vector3(first.x, first.y + lengthDifference, 0);
                            }
                        }
                        else
                        {
                            if (first.x > second.x)
                            {
                                activeLineSegments[j] = new Vector3(first.x - lengthDifference, first.y, 0);
                            }
                            else
                            {
                                activeLineSegments[j] = new Vector3(first.x + lengthDifference, first.y, 0);
                            }
                        }
                        break;
                    }
                }
                if (segmentsToRemove > 0)
                {
                    startIndex += segmentsToRemove;
                    segmentCount -= segmentsToRemove;
                }
            }
            else
            {
                lineLength += segmentLength;
            }
        }
    }

    private static float GetSegmentLength(Vector3 first, Vector3 second)
    {
        // Since the lines are always perpendicular - we can avoid using sqrt
        if (first.x == second.x)
        {
            return Mathf.Abs(first.y - second.y);
        }
        else
        {
            return Mathf.Abs(first.x - second.x);
        }
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

    public void Dispose()
    {
        activeLineSegments.Dispose();
    }
}
