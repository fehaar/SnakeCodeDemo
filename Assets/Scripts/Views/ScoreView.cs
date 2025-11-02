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

    public void Initialize(FoodArea foodArea)
    {
        this.foodArea = foodArea;
        this.foodArea.FoodEatenChanged += FoodArea_FoodEatenChanged;
    }

    private void OnDestroy()
    {
        foodArea.FoodEatenChanged -= FoodArea_FoodEatenChanged;
    }

    private void FoodArea_FoodEatenChanged(int amount)
    {
        label.text = amount.ToString();
    }
}
