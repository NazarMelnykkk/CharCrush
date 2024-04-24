public class GamePopUpViewController : PopUpControllerBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        GameEventHandler.Instance.GameEvents.OnLoseEvent += Open;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEventHandler.Instance.GameEvents.OnLoseEvent -= Open;
    }

}
