using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public Transform[] Path;

    public float MovementSpeed = 1.0f;

    public float Health = 10.0f;

    private int nextTargetIndex;

    public void FixedUpdate()
    {
    }
}
