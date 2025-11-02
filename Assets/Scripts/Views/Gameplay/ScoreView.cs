using TMPro;
using UnityEngine;

/// <summary>
/// This view shows how much food we have eaten
/// </summary>
public class ScoreView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text label;

    private FoodArea foodArea;

    public void Initialize(GameData gameData)
    {
        // If we are replaying, make sure to remove subscription to the old action
        if (this.foodArea != null)
        {
            this.foodArea.FoodEatenChanged -= FoodArea_FoodEatenChanged;
        }

        this.foodArea = gameData.FoodArea;
        this.foodArea.FoodEatenChanged += FoodArea_FoodEatenChanged;
    }

    private void OnDestroy()
    {
        if (foodArea != null)
        {
            foodArea.FoodEatenChanged -= FoodArea_FoodEatenChanged;
        }
    }

    private void FoodArea_FoodEatenChanged(int amount)
    {
        label.text = amount.ToString();
    }
}
