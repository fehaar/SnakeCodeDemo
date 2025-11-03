using Cysharp.Threading.Tasks;
using System;
using System.Linq;

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

    public Snake Snake { get; private set;  }
    public FoodArea FoodArea { get; private set; }
    public GameArea GameArea { get; private set; }

    private List<Func<UniTask>> endGameTaskFactories = new ();

    /// <summary>
    /// Views can register methods here that will supply different end game animations that need to be run before we go back to the menu.
    /// </summary>
    /// <param name="taskFactory"></param>
    public void AddEndGameTaskFactory(Func<UniTask> taskFactory)
    {
        endGameTaskFactories.Add(taskFactory);
    }

    public void EndGame()
    {
        PerformEndGameTasksAndEndGame().Forget();
    }

    private async UniTaskVoid PerformEndGameTasksAndEndGame()
    {
        var tasks = endGameTaskFactories.Select(tf => tf.Invoke()).ToArray();
        await UniTask.WhenAll(tasks);
        OnGameEnded?.Invoke();
    }

    public Action OnGameEnded;
}
