using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    private Tile tileWhite;
    private Tile tileSpace;
    private RuleTile rt_ground;
    [SerializeField] private GameObject groundLayer, boundsLayer;
    public Tilemap groundMap, boundsMap;
    private int boundsMapPad = 5;
    private int currentMapSize = 0;

    private void Awake()
    {
        groundMap = groundLayer.GetComponent<Tilemap>();
        groundMap.CompressBounds();
        boundsMap = boundsLayer.GetComponent<Tilemap>();
        tileWhite = (Tile)Resources.Load("Tiles/basicWhite");
        tileSpace = (Tile)Resources.Load("Tiles/Tile_Space");
        rt_ground = (RuleTile)Resources.Load("Tiles/RT_SandyRock");
        //terrainTiles = Resources.LoadAll("TerrainTiles/SandyRocks");
    }

    public void BuildMap (int _seed, int _mapSize) // replace with 2 overload funct (mapfile arg/ mapsize arg)
    {
        Random.InitState(_seed);
        GenerateMap(_mapSize);
    }

    public void GenerateMap(int _mapSize)
    {
        while(currentMapSize < _mapSize)
        {
            Tilemap randomShape = GetRandomShape();
            randomShape.CompressBounds();

            Vector3Int origin = GetPointInMap(groundMap);

            for (int x = randomShape.cellBounds.xMin; x < randomShape.cellBounds.xMax; x++)
            {
                for (int y = randomShape.cellBounds.yMin; y < randomShape.cellBounds.yMax; y++)
                {
                    Vector3Int adjustePos = new Vector3Int(x + origin[0], y + origin[1], 0);
                    if (randomShape.HasTile(new Vector3Int(x, y, 0)) && !groundMap.HasTile(adjustePos))
                    {
                        groundMap.SetTile(adjustePos, tileWhite);
                        currentMapSize += 1;
                    }
                }
            }
        }
    }

    private Tilemap GetRandomShape()
    {
        Object[] shapeMaps = Resources.LoadAll("ShapeMaps");

        if(shapeMaps.Length <1)
        {
            Debug.LogError("Error: No Shapemaps found!");
            return null;
        }

        int randomMap = Random.Range(0, shapeMaps.Length);
        GameObject shapeMap = (GameObject)shapeMaps[randomMap];
        return shapeMap.GetComponent<Tilemap>();
    }

    public Vector3Int GetPointInMap(Tilemap _map)
    {
        if (currentMapSize <= 0) return _map.origin;

        BoundsInt mapBounds = _map.cellBounds;
        Vector3Int point = new Vector3Int(Random.Range(mapBounds.xMin,mapBounds.xMax), Random.Range(mapBounds.yMin, mapBounds.yMax), 0);

        while (!_map.HasTile(point))
        {
            point = new Vector3Int(Random.Range(mapBounds.xMin, mapBounds.xMax), Random.Range(mapBounds.yMin, mapBounds.yMax), 0);
        }
        return point;
    }

    public void PlaceGround()
    {
        groundMap.SwapTile(tileWhite, rt_ground);
    }
}
