using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] int mapSize;
    [SerializeField] GameObject tileMaps;
    [SerializeField] private GameObject player;
    public static MapManager mapManager;

    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if( Instance == null)
            { Instance = this; }
        else 
            { Debug.LogError("More then one instance of GameManager found!"); }
        mapManager = tileMaps.GetComponent<MapManager>();
    }
    #endregion
    
    void Start()
    {
        LevelSetup();
        player.transform.position = mapManager.groundMap.CellToWorld(mapManager.GetPointInMap(mapManager.groundMap)) + new Vector3(0.5f,0.5f,0);
    }

    void LevelSetup()
    {
        /*if (loadmap):
            Load Map
        else:
            Generate level*/
        mapManager.BuildMap(false, mapSize);
        mapManager.PlaceGround();
    }

}
