using UnityEngine;

[CreateAssetMenu(fileName = "GameAreaSettings", menuName = "Game settings/Game area settings")]
public class GameAreaSettings : ScriptableObject
{
    [SerializeField]
    [Tooltip("The game area size")]
    private Vector2 size = new Vector2(150, 100);

    internal GameArea CreateGameArea()
    {
        return new GameArea(new Bounds(Vector3.zero, size));
    }
}