using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Dictionary<Vector2Int, Tile> GeneratedTiles = new Dictionary<Vector2Int, Tile>();

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
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public struct Tile
    {
        private Vector2Int _tileCoordinates;
        // private Vector2 _cartesianCoordinates;
        //
        // public bool ColliderEnabled;

        public Tile(Vector2Int tileCoordinates)
        {
            this._tileCoordinates = tileCoordinates;
            // this._cartesianCoordinates = TileManager.Instance.GeneratedTiles
        }

        public Vector2Int TileCoordinates
        {
            get { return _tileCoordinates; }
            set { _tileCoordinates = value; }
        }
    }
}
