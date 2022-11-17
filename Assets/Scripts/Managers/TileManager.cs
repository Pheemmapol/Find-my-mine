using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;
    [SerializeField] private Tiles _tilePrefab;
    [SerializeField] private Board _boardPrefab;
    [SerializeField] private Color _normalColor, _pressedColor,_boardColor;
    [SerializeField] private int _width, _height;
    [SerializeField] private float _scale;
    [SerializeField] private Transform _cam;
    [SerializeField] private float _bombcount;



    public static Dictionary<Vector2, Tiles> _tiles;


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

    public void setBoardSize(int width,int height)
    {
        _width = width;
        _height = height;
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

                    spawnedTile.Init(_normalColor, _pressedColor,new Vector2(x,y));

                    _tiles[new Vector2(x, y)] = spawnedTile;
                }
            }

         _cam.transform.position = new Vector3((float)_height / 2 - 0.5f, (float)_width / 2 - 0.5f, -10);
    }

    public static void ResetBoard()
    {
        foreach(var tile in _tiles.Values)
        {
            tile.UnrevealTile();
        }
    }

    public static void RevealTile(Vector2 pos,bool isBomb)
    {
        GetTileFromPosition(pos).revealTile(isBomb);
    }

    public static Tiles GetTileFromPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

}
