using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public readonly Dictionary<Vector2Int, Tile> GeneratedTiles = new Dictionary<Vector2Int, Tile>();

    public static TileManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public struct Tile
    {
        public Tile(Vector2Int tileCoordinates)
        {
            TileCoordinates = tileCoordinates;
        }

        private Vector2Int TileCoordinates { get; set; }
    }
}
