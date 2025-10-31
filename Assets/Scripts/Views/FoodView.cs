using UnityEngine;

/// <summary>
/// This shows us the food that 
/// </summary>
public class FoodView : MonoBehaviour
{
    internal void Initalize(FoodArea foodArea, FoodArea.Food food)
    {
        this.foodArea = foodArea;
        this.food = food;
        transform.position = food.Position;
    }

    private FoodArea foodArea;
    private FoodArea.Food food;

    internal void Eat()
    {
        foodArea.Eat(food);
        Destroy(gameObject);
    }
}