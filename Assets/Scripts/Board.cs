using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    private Color _boardColor;

    public void Init(Color color)
    {
        _renderer.color = _boardColor;
    }
}
