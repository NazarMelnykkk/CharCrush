using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] protected SettingsController _settingsController;
    [SerializeField] protected LVLController _lvlController;

    [SerializeField] protected Button _settingsButton;
    [SerializeField] protected Button _LVLControllerButton;

    private void OnEnable()
    {
        _settingsButton.onClick.AddListener(Settings);
        _LVLControllerButton.onClick.AddListener(LVLController);
    }

    private void OnDisable()
    {
        _settingsButton.onClick.RemoveListener(Settings);
        _LVLControllerButton.onClick.RemoveListener(LVLController);
    }

    private void Settings()
    {
        _settingsController.Open();
    }

    private void LVLController()
    {
        _lvlController.Open();
    }

}
