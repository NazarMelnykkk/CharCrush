using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public enum ObjectType
{
    Red,
    Blue,
    Yellow,
    Green,
    White
}


public class Object : MonoBehaviour
{

    //[Header("Index")]
    public int XIndex { get; private set; }
    public int YIndex { get; private set; }

    private Vector2 _position;
    private Vector2 _targetPosition;

 /*   [Header("Logic")]*/
    public bool IsMoving { get; private set; }
    public bool IsMatched;


    [Header("Components")]
    [SerializeField] private ObjectView _view;
    [SerializeField] private ObjectConfig _config;

    /*    [Header("Config")]*/
    public ObjectType Type;

    public void Constract(ObjectConfig config)
    {
        _config = config;

        Type = _config.ObjectType;
        _view.Constract(_config.Sprite, _config.Color);
        gameObject.name = _config.name;
    }

/*    private void Init()
    {
        Type = _config.ObjectType;
        _view.Constract(_config.Sprite, _config.Color);
        gameObject.name = _config.name;
    }*/

    public void SetIndex(int x, int y)
    {
        XIndex = x;
        YIndex = y;
    }

    public void MoveToTarget(Vector2 targetPosition)
    {
        StartCoroutine(MoveCoroutine(targetPosition));
    }

    private IEnumerator MoveCoroutine(Vector2 targetPosition)
    {
        IsMoving = true;

        float duration = 0.2f;

        Vector2 startPosition = transform.position;
        float elaspedTime = 0f;

        while (elaspedTime < duration)
        {
            float time = elaspedTime / duration;

            transform.position = Vector2.Lerp(startPosition, targetPosition, time);

            elaspedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = targetPosition;
        IsMoving = false;

    }

    public void SetMatched(bool value)
    {
        IsMatched = value;
    }


}
