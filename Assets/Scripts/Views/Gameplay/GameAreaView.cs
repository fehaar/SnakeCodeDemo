using UnityEngine;

/// <summary>
/// Show the boundaries of the play area
/// </summary>
public class GameAreaView : MonoBehaviour
{
    private Snake snake;
    private GameArea gameArea;

    public void Initialize(GameData gameData)
    {
        this.snake = gameData.Snake;
        this.gameArea = gameData.GameArea;
        transform.localScale = gameArea.Size;
    }

    private void Update()
    {
        if (this.gameArea != null)
        {
            gameArea.KillSnakeOutsideArea(snake);
        }
    }
}
