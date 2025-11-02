using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This is a simple end game menu 
/// </summary>
public class EndMenuView : GameDataInitializable
{
    [SerializeField]
    private Button playAgainButton;
    [SerializeField]
    private TMPro.TMP_Text score;

    private GameData gameData;

    void Start()
    {
        playAgainButton.onClick.AddListener(PlayAgain);
    }

    private void PlayAgain()
    {
        gameObject.SetActive(false);

        var startMenu = FindFirstObjectByType<StartMenuView>(FindObjectsInactive.Include);
        startMenu.gameObject.SetActive(true);
    }

    public override void Initialize(GameData gameData)
    {
        this.gameData = gameData;
        this.gameData.OnGameEnded += OnGameEnded;
    }

    private void OnGameEnded()
    {
        gameObject.SetActive(true);
        gameData.OnGameEnded -= OnGameEnded;
        score.text = gameData.FoodArea.FoodEaten.ToString();
    }
}
