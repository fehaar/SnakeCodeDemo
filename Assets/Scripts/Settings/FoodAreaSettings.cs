using UnityEngine;

[CreateAssetMenu(fileName = "FoodAreaSettings", menuName = "Game settings/Food area settings")]
public class FoodAreaSettings : ScriptableObject
{
    [SerializeField]
    private float foodSpawnInterval = 5f;
    [SerializeField]
    private int maxFood = 6;

    internal FoodArea CreateFoodArea(GameArea gameArea)
    {
        return new FoodArea(foodSpawnInterval, maxFood, gameArea);
    }
}
