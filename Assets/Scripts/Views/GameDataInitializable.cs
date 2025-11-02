using UnityEngine;

/// <summary>
/// This is a base class for behaviours that will get initialized by game data.
/// This is so we can avoid having direct references to a lot of other behaviours in the scene.
/// </summary>
public abstract class GameDataInitializable : MonoBehaviour
{
    public abstract void Initialize(GameData gameData);
}
