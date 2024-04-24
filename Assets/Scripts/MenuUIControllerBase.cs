using UnityEngine;

public class MenuUIControllerBase : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected GameObject _container;

    public virtual void Open()
    {
        _container.SetActive(true);
    }
}
