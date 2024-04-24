using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _image;
    [SerializeField] private Animator _animator;

    public void Constract(Sprite sprite, Color color)
    {
        SetSprite(sprite, color);
    }

    private void SetSprite(Sprite sprite, Color color)
    {
        _image.sprite = sprite;
        _image.color = color;
    }
}
