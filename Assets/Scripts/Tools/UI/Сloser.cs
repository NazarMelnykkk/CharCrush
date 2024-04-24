using UnityEngine;
using UnityEngine.UI;

public class Сloser : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private GameObject[] _objectsToClose;

    private void Awake()
    {
        if (_closeButton == null)
        {
            _closeButton = GetComponentInChildren<Button>();
        }
    }

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        foreach (var obj in _objectsToClose)
        {
            obj.SetActive(false);
        }
    }
}
