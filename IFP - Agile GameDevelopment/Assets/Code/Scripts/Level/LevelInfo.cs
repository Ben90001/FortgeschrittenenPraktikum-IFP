using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [Min(5)]
    public int PlayerLives;

    public Bounds GameplayArea;

    public Wave[] Waves;
}
