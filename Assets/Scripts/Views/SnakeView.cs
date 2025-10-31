using System;
using UnityEngine;

/// <summary>
/// This is the visual representation of a Snake in the game.
/// </summary>
public class SnakeView : MonoBehaviour
{
    private Snake snake;
    private LineRenderer line;

    public void Initialize(Snake snake)
    {
        this.snake = snake;
        this.line = GetComponent<LineRenderer>();
    }

    void Start()
    {
        line.SetPosition(0, new Vector3(snake.Position.x, snake.Position.y, 0));
        line.SetPosition(1, new Vector3(snake.Position.x, snake.Position.y, 0));
    }

    // Update is called once per frame
    void Update()
    {
        // Tick the snake's logic
        snake.Tick(Time.deltaTime);

        // Update the visual representation
        line.SetPosition(line.positionCount - 1, new Vector3(snake.Position.x, snake.Position.y, 0));

        // Cull the tail of the snake by looking at it from the head 
        AdjustTailLength();
    }

    /// <summary>
    /// Since the history of the snake is only held in the line renderer, we will have to do correction of the snake directly in the points of the renderer
    /// </summary>
    private void AdjustTailLength()
    {
        var lineLength = 0f;
        for (int i = line.positionCount - 1; i >= 1; i--)
        {
            // Sum up the length of the snake
            var first = line.GetPosition(i);
            var second = line.GetPosition(i - 1);
            var segmentLength = 0f;
            segmentLength = GetSegmentLength(first, second);
            var lengthDifference = lineLength + segmentLength - snake.Length;
            if (lengthDifference > 0)
            {
                // We are too long and need to move our tail a bit
                // We start from the back and see how much to cut off
                var segmentsToRemove = 0;
                for (int j = 0; j < line.positionCount - 1; j++)
                {
                    first = line.GetPosition(j);
                    second = line.GetPosition(j + 1);
                    segmentLength = GetSegmentLength(first, second);
                    if (lengthDifference > segmentLength)
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
                                line.SetPosition(j, new Vector3(first.x, first.y - lengthDifference, 0));
                            }
                            else
                            {
                                line.SetPosition(j, new Vector3(first.x, first.y + lengthDifference, 0));
                            }
                        }
                        else
                        {
                            if (first.x > second.x)
                            {
                                line.SetPosition(j, new Vector3(first.x - lengthDifference, first.y, 0));
                            }
                            else
                            {
                                line.SetPosition(j, new Vector3(first.x + lengthDifference, first.y, 0));
                            }
                        }
                        break;
                    }
                }
                if (segmentsToRemove > 0)
                {
                    // Do the segment removal in one array copy
                    // TODO: We could optimize this by not making new arrays if they are of the same length
                    var oldPositions = new Vector3[line.positionCount];
                    line.GetPositions(oldPositions);
                    var newPositions = new Vector3[line.positionCount - segmentsToRemove];
                    Array.Copy(oldPositions, segmentsToRemove, newPositions, 0, newPositions.Length);
                    line.positionCount = newPositions.Length;
                    line.SetPositions(newPositions);
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

    internal void Turn(Snake.MoveDirection moveDirection)
    {
        // We will add in a new segment of the line when we turn
        line.positionCount++;        
        snake.Turn(moveDirection);
    }
}
