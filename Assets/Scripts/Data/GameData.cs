using System;

/// <summary>
/// This holds all the data for a game session and has actions to signal when something happens to the game.
/// </summary>
public class GameData
{
    public GameData(SnakeSettings snakeSettings, GameAreaSettings gameAreaSettings, FoodAreaSettings foodAreaSettings)
    {
        Snake = snakeSettings.CreateSnake();
        GameArea = gameAreaSettings.CreateGameArea();
        FoodArea = foodAreaSettings.CreateFoodArea(GameArea);
    }

    /// Data fields
    
    public Snake Snake { get; private set;  }
    public FoodArea FoodArea { get; private set; }
    public GameArea GameArea { get; private set; }

    /// 

    public void EndGame()
    {
        OnGameEnded?.Invoke();
    }

    /// Actions

    public Action OnGameEnded;
}
