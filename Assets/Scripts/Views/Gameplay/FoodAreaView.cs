using UnityEngine;

/// <summary>
/// Show the food in the food area and check if the snake eats any of the food
/// </summary>
public class FoodAreaView : MonoBehaviour
{
    [SerializeField]
    private FoodView foodPrefab;

    private FoodArea foodArea;
    private Snake snake;

    public void Initialize(GameData gameData)
    {
        this.foodArea = gameData.FoodArea;
        this.snake = gameData.Snake;
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
