/// <summary>
/// Show the boundaries of the play area
/// </summary>
public class GameAreaView : GameDataInitializable
{
    private Snake snake;
    private GameArea gameArea;

    public override void Initialize(GameData gameData)
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
