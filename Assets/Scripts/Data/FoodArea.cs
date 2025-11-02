using UnityEngine;
/// <summary>
/// This is the area that holds the food that the snake can eat.
/// Food spawns in random intervals and when a snake eats them it will grow longer and move faster
/// </summary>
public class FoodArea
{
    public FoodArea(float spawnInterval, int maxFood, GameArea gameArea)
    {
        this.spawnInterval = spawnInterval;
        spawnTimeLeft = spawnInterval / 2f;
        this.maxFood = maxFood;
        this.gameArea = gameArea;
    }
    public int FoodEaten { get; private set; } = 0;

    public event System.Action<int> FoodEatenChanged;

    private float spawnInterval;
    private int maxFood;
    private GameArea gameArea;
    private List<Food> availableFood = new();

    const float DISTANCE_FROM_EDGE = 3f;

    private float spawnTimeLeft;

    public class Food
    {
        public Food(Vector2 position)
        {
            Position = position;
        }

        public Vector2 Position { get; private set;  }
    }

    public Food Tick(float deltaTime)
    {
        if (availableFood.Count < maxFood && (spawnTimeLeft -= deltaTime) < 0)
        {
            // Spawn a new piece of food within the game area
            spawnTimeLeft = spawnInterval;
            var food = new Food(new Vector2(Random.Range(gameArea.Min.x + DISTANCE_FROM_EDGE, gameArea.Max.x - DISTANCE_FROM_EDGE), Random.Range(gameArea.Min.y + DISTANCE_FROM_EDGE, gameArea.Max.y - DISTANCE_FROM_EDGE)));
            availableFood.Add(food);
            return food;
        }
        return null;
    }

    internal void Eat(Food food)
    {
        availableFood.Remove(food);
        FoodEaten++;
        FoodEatenChanged?.Invoke(FoodEaten);
    }
}