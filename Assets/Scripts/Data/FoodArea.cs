using UnityEngine;
/// <summary>
/// This is the area that holds the food that the snake can eat.
/// Food spawns in random intervals and when a snake eats them it will grow longer and move faster
/// </summary>
public class FoodArea
{
    public FoodArea(float spawnInterval, int maxFood, Boundaries boundaries)
    {
        this.spawnInterval = spawnInterval;
        spawnTimeLeft = spawnInterval / 2f;
        this.maxFood = maxFood;
        this.boundaries = boundaries;
    }

    private float spawnInterval;
    private int maxFood;
    private Boundaries boundaries;
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
            spawnTimeLeft = spawnInterval;
            var food = new Food(new Vector2(Random.Range(boundaries.Min.x + DISTANCE_FROM_EDGE, boundaries.Max.x - DISTANCE_FROM_EDGE), Random.Range(boundaries.Min.x + DISTANCE_FROM_EDGE, boundaries.Max.x - DISTANCE_FROM_EDGE)));
            return food;
        }
        return null;
    }
}