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
    private int pointCount = 2;
    // Note that we are using an array here for performance that we roll along in when turning and then start from the beginning
    // in the reserve array when we reach the end. Since it has a limited size we can only have 1000 turns active on screen.
    // Hopefully that is enough otherwise we should bump the size.
    private NativeArray<Vector3> activeLineSegments = new NativeArray<Vector3>(1000, Allocator.Domain);
    private NativeArray<Vector3> reserveLineSegments = new NativeArray<Vector3>(1000, Allocator.Domain);

    /// <summary>
    /// Which way is the snake moving?
    /// </summary>
    public MoveDirection CurrentDirection { get; set; } = MoveDirection.Right;

    public NativeSlice<Vector3> Positions => activeLineSegments.Slice(startIndex, pointCount);

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
        activeLineSegments[startIndex + pointCount - 1] = Position;
        AdjustTailLength(Length);
        CheckCollisionWithSelf();
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
        if (startIndex + pointCount == activeLineSegments.Length)
        {
            // Copy the active array to the start of the reserve
            NativeArray<Vector3>.Copy(activeLineSegments, startIndex, reserveLineSegments, 0, pointCount);
            startIndex = 0;
            // And then swap the two arrays
            var tmp = activeLineSegments;
            activeLineSegments = reserveLineSegments;
            reserveLineSegments = tmp;
        }
        pointCount++;

        activeLineSegments[startIndex + pointCount - 1] = Position;
    }

    internal void AdjustTailLength(float totalLength)
    {
        var lineLength = 0f;
        for (int i = startIndex + pointCount - 1; i >= startIndex + 1; i--)
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
                for (int j = startIndex; j < startIndex + pointCount - 1; j++)
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
                    pointCount -= segmentsToRemove;
                }
            }
            else
            {
                lineLength += segmentLength;
            }
        }
    }

    /// <summary>
    /// If the snake collides with it's own tail it will die
    /// </summary>
    internal void CheckCollisionWithSelf()
    {
        // This is used to give some slack to the area we collide in
        const float COLLISION_DISTANCE = 0.5f;

        // As we are moving perpendicularly, it is impossible to collide with the two line segmensts closest to the head
        // So we need to have more than 
        if (pointCount > 3)
        {
            for (int i = startIndex; i < startIndex + pointCount - 3; i++)
            {
                var first = activeLineSegments[i];
                var second = activeLineSegments[i + 1];
                if (first.x == second.x)
                {
                    // This is a vertical line - is our head x close enough to the line
                    if (Math.Abs(Position.x - first.x) <= COLLISION_DISTANCE)
                    {
                        if (Position.y < Math.Max(first.y, second.y) && Position.y > Math.Min(first.y, second.y))
                        {
                            Kill();
                        }
                    }
                }
                else
                {
                    // This is a horizontal line - is our head y close enough to the line
                    if (Math.Abs(Position.y - first.y) <= COLLISION_DISTANCE)
                    {
                        if (Position.x < Math.Max(first.x, second.x) && Position.x > Math.Min(first.x, second.x))
                        {
                            Kill();
                        }
                    }
                }
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
        reserveLineSegments.Dispose();
    }
}
