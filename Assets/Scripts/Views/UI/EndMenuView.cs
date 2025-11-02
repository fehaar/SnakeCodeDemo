using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This is a simple end game menu 
/// </summary>
public class EndMenuView : MonoBehaviour
{
    [SerializeField]
    private Button playAgainButton;
    [SerializeField]
    private TMPro.TMP_Text score;

    void Start()
    {
        playAgainButton.onClick.AddListener(PlayAgain);
    }

    private void PlayAgain()
    {
        gameObject.SetActive(false);

        var startMenu = FindFirstObjectByType<StartMenuView>();
        startMenu.gameObject.SetActive(true);
    }
}
