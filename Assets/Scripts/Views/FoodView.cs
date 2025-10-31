using UnityEngine;

public class FoodView : MonoBehaviour
{
    internal void Initalize(FoodArea.Food food)
    {
        transform.position = food.Position;
    }
}