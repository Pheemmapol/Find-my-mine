using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private SpriteRenderer _renderer;
    private Color _normalDTileColor, _normalLTileColor, _pressedDTileColor, _pressedLTileColor;
    private bool hasPressed = false;
    private bool hasFlagged;
    private Vector2 position;
   

    private Dictionary<int, GameObject> numberSprites;
    [SerializeField] private GameObject _bomb;
    [SerializeField] private GameObject _superbomb;

    [SerializeField] private GameObject _one;
    [SerializeField] private GameObject _two;
    [SerializeField] private GameObject _three;
    [SerializeField] private GameObject _four;
    [SerializeField] private GameObject _five;
    [SerializeField] private GameObject _six;
    [SerializeField] private GameObject _seven;
    [SerializeField] private GameObject _eight;
   

    public void Init(Color normalDcolor, Color normalLcolor, Color pressedDcolor, Color pressedLcolor, Vector2 pos) { 

        
        _normalDTileColor = normalDcolor;
        _normalLTileColor = normalLcolor;
        _pressedDTileColor = pressedDcolor;
        _pressedLTileColor = pressedLcolor;
        SetToNormalcolor();
        position = pos;
        numberSprites = new Dictionary<int, GameObject>(){
            {1,_one},
            {2,_two},
            {3,_three},
            {4,_four},
            {5,_five},
            {6,_six},
            {7,_seven},
            {8,_eight}
        };

    }

    public void SetToNormalcolor()
    {
        if (Client.instance.darkmode)
        {
            _renderer.color = _normalDTileColor;
          
        }
        else
        {
            _renderer.color = _normalLTileColor;
        }
        
        hasPressed = false;
    }
    public void SetToPressedcolor()
    {
        if (Client.instance.darkmode)
        {
            _renderer.color = _pressedDTileColor;

        }
        else
        {
            _renderer.color = _pressedLTileColor;
        }
        
        hasPressed = true;
    }


    void OnLeftClick()
    {
        if (!hasPressed && !hasFlagged)
        {
            hasPressed = true;
            //click
            GameManager.Instance.ChangeState(GameManager.GameState.EnemyTurn);
            ClientSend.SendClickPos((int)position.x, (int)position.y);
        }
    }


    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.State == GameManager.GameState.PlayerTurn)
        {
            OnLeftClick();
        }
    }
    
    public void revealTile(int tiletype)
    {
        Explode();
        SetToPressedcolor();
        switch (tiletype)
        {
            case 1:
                showBomb(false);
                break;
            case 2:
                showBomb(true);
                break;
        }

    }

    public void revealTile(int tiletype,int num)
    {
        Explode();
        revealTile(tiletype);
        if(tiletype == 0 && num >0)showNumber(num);
    }

    public void UnrevealTile()
    {
        SetToNormalcolor();
        _highlight.SetActive(false);
        hideAll();
    }
    void OnMouseEnter()
    {
        if (!hasPressed && GameManager.Instance.State == GameManager.GameState.PlayerTurn)
        {
            _highlight.SetActive(true);
        }
    }
    void OnMouseExit()
    {
        if (!hasPressed)
        {
            _highlight.SetActive(false);
        }
    }

    void showBomb(bool superbomb)
    {
        if (superbomb)
        {
            _superbomb.SetActive(true);
        }
        else
        {
            _bomb.SetActive(true);
        }
    }

    void showNumber(int num)
    {
        numberSprites[num].SetActive(true);
    }
    
    void hideAll()
    {
        _bomb.SetActive(false);
        _superbomb.SetActive(false);
        foreach(GameObject number in numberSprites.Values)
        {
            number.SetActive(false);
        }
    }
    public void Explode()
    {
        GameObject newExplosion = Instantiate(TileManager.instance._explosion, this.transform.position + new Vector3(0,0,1),Quaternion.identity);
    }
}
