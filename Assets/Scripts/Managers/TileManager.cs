using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;
    [SerializeField] private Tiles _tilePrefab;
    [SerializeField] private Board _boardPrefab;
    [SerializeField] private Color _normalDarkColor, _normalLightColor, _pressedDarkColor, _pressedLightColor, _boardColor;
    [SerializeField] private int _width, _height;
    [SerializeField] private float _scale;
    [SerializeField] private Transform _cam;
    [SerializeField] private float _bombcount;
    public GameObject _explosion;
    

    public Camera mainCamera;
    public Color newColor;

    public static Dictionary<Vector2, Tiles> _tiles;

    private void Start()
    {
        StartGame(Client.boardinfo.width, Client.boardinfo.height);
        setColor();
    }

    private void Awake()
    {
        instance = this;
    }
    public void StartGame(int width, int height)
    {
        setBoardSize(width, height);
        generateGrid();
        GameUIManager.instance.UpdateScore("0", "0");
    }

    public void setColor()
    {
        Color darkColor = new Color(0.2f, 0.19f, 0.19f, 0);
        Color lightColor = new Color(0.68f, 0.67f, 0.67f, 0);
        if (Client.instance.darkmode)
        { 
            mainCamera.backgroundColor = darkColor;
        }
        else
        {
            mainCamera.backgroundColor = lightColor;
        }
    }
    public void setBoardSize(int width,int height)
    {
        _width = width;
        _height = height;
        _scale = 7.2f / width;
    }
     
    void generateGrid()
    {
        //make board
        var Board = Instantiate(_boardPrefab);
        Board.Init(_boardColor);
        Board.transform.localScale = new Vector3(_height,_width);
        Board.transform.position = new Vector3((float)(_height-1) /2, (float)(_width-1) /2);


        //make grid
        _tiles = new Dictionary<Vector2, Tiles>();
         for (int x = 0; x < _height; x++)
            {
                for (int y = 0; y < _width; y++)
                {
                    var spawnedTile = Instantiate(_tilePrefab, new Vector3(x*_scale, y*_scale), Quaternion.identity);
                    spawnedTile.transform.localScale = new Vector3(_scale-0.1f,_scale-0.1f,1);
                    spawnedTile.GetComponent<Renderer>().sortingOrder = 0;
                    spawnedTile.name = $"Square {x} {y}";

                    spawnedTile.Init(_normalDarkColor,_normalLightColor, _pressedDarkColor, _pressedLightColor, new Vector2(x,y));
                    
                    _tiles[new Vector2(x, y)] = spawnedTile;
                }
            }

         _cam.transform.position = new Vector3((float)6 / 2 - 0.5f, (float)6 / 2 - 0.5f, -10);
    }

    public static void ResetBoard()
    {
        foreach(var tile in _tiles.Values)
        {
            tile.UnrevealTile();
        }
        GameUIManager.instance.HideGameOver();
    }

    public static void RevealTile(Vector2 pos,int tiletype)
    {
        GetTileFromPosition(pos).revealTile(tiletype);
    }

    public static void RevealTile(Vector2 pos, int tiletype,int bombnum)
    {
        GetTileFromPosition(pos).revealTile(tiletype,bombnum);
    }

    public static Tiles GetTileFromPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
    

}
