using System;
using UnityEngine;

public class HexagonalLandscapeGenerator : MonoBehaviour
{
    [SerializeField] private float hexRadius = 1;

    [SerializeField, Range(0.0f, 0.1f)] private float noiseScale1 = 1;
    [SerializeField, Range(0.0f, 0.1f)] private float noiseScale2 = 1;

    [SerializeField] private float minHeight = -1f;
    [SerializeField] private float maxHeight = 3f;

    [SerializeField] private float heightMultiplier = 1;

    [SerializeField, Range(1, 25)] private int generationRadius = 4;

    [SerializeField] private Transform playerTransform;

    private Vector2Int _playerTilePos;
    private Vector2Int _previousPlayerTilePos;

    [SerializeField] private GameObject hexagonPrefab;

    private static HexagonalLandscapeGenerator Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        hexagonPrefab = Resources.Load<GameObject>("Prefabs/Hexagon");

        GenerateLandscape(Vector2.zero, generationRadius);
    }

    private void Update()
    {
        Vector2 playerPos = GetPlayerPosition();

        _playerTilePos.x =
            Mathf.RoundToInt(playerPos.x / (hexRadius * Mathf.Sqrt(3)) - playerPos.y / (hexRadius * Mathf.Sqrt(3)));
        _playerTilePos.y = Mathf.RoundToInt(playerPos.y / (hexRadius * 3f));

        if (_playerTilePos == _previousPlayerTilePos) return;

        GenerateLandscape(new Vector2(playerPos.x * (0.57735f / hexRadius), playerPos.y * (0.66666f / hexRadius)),
            generationRadius);

        _previousPlayerTilePos = _playerTilePos;
    }

    private Vector2 GetPlayerPosition()
    {
        Vector3 playerPos = playerTransform.position;
        return new Vector2(playerPos.x, playerPos.z);
    }

    private void GenerateLandscape(Vector2 centerPoint, int radius)
    {
        for (int x = Mathf.RoundToInt(centerPoint.x - radius); x < Mathf.RoundToInt(centerPoint.x + radius); x++)
        {
            for (int y = Mathf.RoundToInt(centerPoint.y - radius); y < Mathf.RoundToInt(centerPoint.y + radius); y++)
            {
                Vector3 prefabSpawnPos = HexagonalToCartesianCoords(x, y);

                float perlinNoise1 = Mathf.PerlinNoise(prefabSpawnPos.x * noiseScale1, prefabSpawnPos.z * noiseScale1) *
                                     hexRadius;
                float perlinNoise2 = Mathf.PerlinNoise(prefabSpawnPos.x * noiseScale2, prefabSpawnPos.z * noiseScale2) *
                                     hexRadius;

                prefabSpawnPos.y = Mathf.Round(Mathf.Clamp((perlinNoise1 - perlinNoise2) * heightMultiplier, minHeight,
                    maxHeight) * 10f) / 10f;

                Vector2Int tilePos = new Vector2Int(x, y);

                if (!TileManager.Instance.GeneratedTiles.ContainsKey(tilePos))
                {
                    GameObject hexagon = Instantiate(hexagonPrefab, prefabSpawnPos, Quaternion.identity, transform);
                    hexagon.transform.localScale = new Vector3(hexRadius, hexRadius, hexRadius);

                    TileManager.Instance.GeneratedTiles.Add(tilePos, new TileManager.Tile(tilePos));
                }
            }
        }
    }

    private Vector3 HexagonalToCartesianCoords(int x, int y)
    {
        double xPos = hexRadius * Math.Cos(30 * Mathf.Deg2Rad) * (x * 2 + y % 2);
        double yPos = hexRadius * (3.0 / 2.0) * y;

        return new Vector3((float)xPos, 0, (float)yPos);
    }
}