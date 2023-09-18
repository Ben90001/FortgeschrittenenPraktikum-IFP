using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public Tilemap tilemap;

    public TileBase grass;
    public TileBase mountain;
    public TileBase path;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3    mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mouseTilePosition  = tilemap.WorldToCell(mouseWorldPosition);

            TileBase tile = tilemap.GetTile(mouseTilePosition);

            if (tile == grass)
            {
                Debug.Log("Clicked grass.");
            }
        }
    }
}
