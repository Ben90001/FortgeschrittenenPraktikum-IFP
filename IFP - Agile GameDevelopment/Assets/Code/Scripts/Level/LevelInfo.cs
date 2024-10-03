using UnityEngine;

/// <summary>
/// Holds data for each level.
/// </summary>
public class LevelInfo : MonoBehaviour
{
    /// <summary>
    /// The initial player lives.
    /// </summary>
    [Min(5)]
    public int PlayerLives;

    /// <summary>
    /// The area in which towers can be placed.
    /// </summary>
    public Bounds GameplayArea;

    /// <summary>
    /// The waves the level contains.
    /// </summary>
    public Wave[] Waves;
}
