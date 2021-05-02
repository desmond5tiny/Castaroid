using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    private Tile tileWhite;
    private Tile tileSpace;
    private RuleTile rt_ground;
    [SerializeField] private GameObject groundLayer, bgLayer, playerShipLayer;
    private Tilemap groundMap;
    private int boundsMapPad = 8;
    private int currentMapSize = 0;
    private Vector3 startPos;

    private void Awake()
    {
        groundMap = groundLayer.GetComponent<Tilemap>();
        groundMap.CompressBounds();
        tileWhite = (Tile)Resources.Load("Tiles/basicWhite");
        tileSpace = (Tile)Resources.Load("Tiles/Tile_Space");
        rt_ground = (RuleTile)Resources.Load("Tiles/RT_SandyRock");
    }

    public void BuildMap (int _seed, int _mapSize) // replace with 2 overload funct (mapfile arg/ mapsize arg)
    {
        Random.InitState(_seed);
        GenerateMap(_mapSize);
    }

    public void GenerateMap(int _mapSize)
    {
        
        GenGround(_mapSize);
        GenBackground();
        MakeStartPoint();

        /*for (int i = 0; i < 5; i++) // gets a point closer to the edge of the map
        {
            Vector3 newPos = GetPointInMap(groundMap);
            if (Vector3.Distance(Vector3.zero, newPos) > Vector3.Distance(Vector3.zero, startPos))
            {
                startPos = newPos;
            }
        }*/
    }
    private void GenGround(int _mapSize)
    {
        while (currentMapSize < _mapSize)
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
        groundMap.SwapTile(tileWhite, rt_ground);
    }

    private void GenBackground()
    {
        Tilemap bgTilemap = bgLayer.GetComponent<Tilemap>();
        for (int x = groundMap.cellBounds.xMin - boundsMapPad; x < groundMap.cellBounds.xMax + boundsMapPad; x++)
        {
            for (int y = groundMap.cellBounds.yMin - boundsMapPad; y < groundMap.cellBounds.yMax + boundsMapPad; y++)
            {
                bgTilemap.SetTile(new Vector3Int(x, y, 0), tileSpace);
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

    private void MakeStartPoint()
    {
        Vector3Int point = GetPointInMap(groundMap);

        while (!CheckStartPos(point))
        {
            point = GetPointInMap(groundMap);
        }

        startPos = groundMap.CellToWorld(point);
    }

    private bool CheckStartPos(Vector3Int pos)
    {
        for (int x = pos.x-2; x < pos.x+2; x++)
        {
            for (int y = pos.y-2; y < pos.y+2; y++)
            {
                if (!groundMap.HasTile(new Vector3Int(x,y,0)))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public Vector3 GetStartPos() { return startPos; }
}
