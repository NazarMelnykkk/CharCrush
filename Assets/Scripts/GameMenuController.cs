public class GameMenuController : PopUpControllerBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        GameEventHandler.Instance.GameEvents.OnPauseEvent += Open;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEventHandler.Instance.GameEvents.OnPauseEvent -= Open;
    }
}
