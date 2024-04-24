using UnityEngine;
using UnityEngine.UI;

public class PopUpControllerBase : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected GameObject _container;
    [SerializeField] protected GameObject _fade;

    [SerializeField] protected Button _menuButton;
    [SerializeField] protected Button _restartButton;

    protected string _menuScene = "Menu";

    protected virtual void OnEnable()
    {
        _menuButton.onClick.AddListener(Menu);
        _restartButton.onClick.AddListener(Restart);
    }

    protected virtual void OnDisable()
    {
        _menuButton.onClick.RemoveListener(Menu);
        _restartButton.onClick.RemoveListener(Restart);
    }

    protected void Open()
    {
        _container.SetActive(true);
        _fade.SetActive(true);
    }

    protected void Restart()
    {
        GameEventHandler.Instance.GameEvents.Restart();
    }

    protected void Menu()
    {
        SceneLoader.Instance.Transition(_menuScene, gameObject.scene.name);
    }
}
