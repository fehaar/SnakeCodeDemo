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

    public void Initialize(FoodArea foodArea, Snake snake)
    {
        this.foodArea = foodArea;
        this.snake = snake;
    }

    private void Update()
    {
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
