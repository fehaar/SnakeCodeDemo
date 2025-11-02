using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

/// <summary>
/// Show the food in the food area and check if the snake eats any of the food
/// </summary>
public class FoodAreaView : GameDataInitializable
{
    [SerializeField]
    private FoodView foodPrefab;
    [SerializeField]
    private TweenSettings<float> foodDeathAnimation;

    private GameData gameData;
    private FoodArea foodArea;
    private Snake snake;

    public override void Initialize(GameData gameData)
    {
        this.gameData = gameData;
        this.gameData.OnGameEnded += OnGameEnded;
        this.gameData.AddEndGameTaskFactory(EndGameAnimation);
        this.foodArea = gameData.FoodArea;
        this.snake = gameData.Snake;
    }

    private async UniTask EndGameAnimation()
    {
        var foodAnimations = new List<UniTask>();
        foreach (var leftoverFood in GetComponentsInChildren<FoodView>())
        {
            var localSettings = foodDeathAnimation;
            localSettings.settings.startDelay = Random.Range(0f, localSettings.settings.startDelay);
            foodAnimations.Add(Tween.Scale(leftoverFood.transform, localSettings)
                                .OnComplete(target: leftoverFood.gameObject, (target) => Destroy(target))
                                .ToYieldInstruction()
                                .ToUniTask());
        }
        await UniTask.WhenAll(foodAnimations);
    }

    private void OnGameEnded()
    {
        this.gameData.OnGameEnded -= OnGameEnded;
    }

    private void Update()
    {
        // Check if the game is started at all
        if (snake == null)
        {
            return;
        }
        // If the snake is dead, don't spawn more food
        if (snake.IsDead)
        {
            return;
        }
        var food = foodArea.Tick(Time.deltaTime);
        if (food != null)
        {
            var foodView = Instantiate(foodPrefab, transform);
            foodView.Initalize(foodArea, food);
        }
    }
}
