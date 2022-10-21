using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private SpriteRenderer _renderer;
    private Color _normalTileColor,_pressedTileColor;
    private bool hasPressed;
    private bool hasFlagged;
    private Vector2 position;

    [SerializeField] private GameObject _bomb;


    public void Init(Color normalcolor,Color pressedcolor, Vector2 pos) { 
        _renderer.color = normalcolor;
        _normalTileColor = normalcolor;
        _pressedTileColor = pressedcolor;
        position = pos;

    }

    public void SetToNormalcolor()
    {
        _renderer.color = _normalTileColor;
        hasPressed = false;
    }
    public void SetToPressedcolor()
    {
        _renderer.color = _pressedTileColor;
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

    public void revealTile(bool isBOMB)
    {
        SetToPressedcolor();
        if (isBOMB)
        {
            _bomb.SetActive(true);
        }

    }

    public void UnrevealTile()
    {
        SetToNormalcolor();
        _highlight.SetActive(false);
        _bomb.SetActive(false);
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

}
