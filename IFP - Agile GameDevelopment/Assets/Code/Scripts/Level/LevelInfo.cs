using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [Min(5)]
    public int playerLives;

    public Bounds GameplayArea;

    public Wave[] Waves;
}
