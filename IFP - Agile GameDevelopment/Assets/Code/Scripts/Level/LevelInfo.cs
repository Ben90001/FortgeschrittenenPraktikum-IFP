using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public Bounds GameplayArea;

    public Wave[] Waves;

    [HideInInspector]
    public Transform[] Waypoints;

    public void Awake()
    {
        Transform path = transform.Find("Path");

        if (path != null) 
        {
            Waypoints = new Transform[path.childCount];

            for (int i = 0; i < Waypoints.Length; i++) 
            {
                Waypoints[i] = path.GetChild(i);
            }
        }
    }
}
