using UnityEngine;

[CreateAssetMenu(menuName = "Config/Object")]
public class ObjectConfig : ScriptableObject
{
    public Sprite Sprite;
    public ObjectType ObjectType;
    public Color Color;

    private void OnValidate()
    {
        switch (ObjectType)
        {
            case ObjectType.Red:
                Color = Color.red;
                break;
            case ObjectType.Blue:
                Color = Color.blue;
                break;
            case ObjectType.Yellow:
                Color = Color.yellow;
                break;
            case ObjectType.Green:
                Color = Color.green;
                break;
            case ObjectType.White:
                Color = Color.white;
                break;
            default:
                break;
        }
    }
}
