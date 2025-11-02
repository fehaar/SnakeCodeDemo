using UnityEngine;

[CreateAssetMenu(fileName = "SnakeSettings", menuName = "Game settings/Snake settings")]
public class SnakeSettings : ScriptableObject
{
    [SerializeField]
    [Tooltip("What is the starting speed of the snake")]
    private float snakeStartingSpeed = 1f;
    [SerializeField]
    [Tooltip("What is the starting length of the snake")]
    private int snakeStartingLength = 20;
    [SerializeField]
    [Tooltip("How much does the snake increase in length when it eats")]
    private int snakeLengthIncrement = 20;
    [SerializeField]
    [Tooltip("How much does the snake increase in speed when it eats")]
    private float snakeSpeedIncrement = 20;

    public Snake CreateSnake()
    {
        return new Snake(snakeStartingSpeed,  snakeStartingLength, snakeLengthIncrement, snakeSpeedIncrement);
    }
}
