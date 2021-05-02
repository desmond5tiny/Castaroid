using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int mapSize;
    [SerializeField] private float startSpawnRadius = 3;
    [SerializeField] GameObject tileMaps;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private bool generateNewSeed = false;
    
    private int gameSeed;
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
        if (generateNewSeed) //makes a new seed and stores it in playerprefs
        {
            gameSeed = (int)DateTime.Now.Ticks;
            Debug.Log(gameSeed);
            PlayerPrefs.SetInt("gameSeed", gameSeed);
        }
        else if (PlayerPrefs.HasKey("gameSeed")) { gameSeed = PlayerPrefs.GetInt("gameSeed"); }
        else { Debug.LogError("No Game Seed found!"); } //throws error if no seed was found and was supposed to be retrieved
    }
    #endregion
    
    void Start()
    {
        LevelSetup();
    }

    void LevelSetup()
    {
        mapManager.BuildMap(gameSeed, mapSize);
        playerShip.transform.position = mapManager.GetStartPos() + new Vector3(0, 0, 0);
        player.transform.position = mapManager.GetStartPos() + new Vector3(0.5f, -0.5f);
    }

}
