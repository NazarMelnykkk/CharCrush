using UnityEngine;

public class GameController : MonoBehaviour
{
/*    [Header("Components")]
    [SerializeField] private*/


    private void OnEnable()
    {
        GameEventHandler.Instance.GameEvents.OnPlayEvent += Play;
        GameEventHandler.Instance.GameEvents.OnPauseEvent += Pause;
        GameEventHandler.Instance.GameEvents.OnRestartEvent += Restart;
        GameEventHandler.Instance.GameEvents.OnFinishEvent += Finish;
    }

    private void OnDisable()
    {
        GameEventHandler.Instance.GameEvents.OnPlayEvent -= Play;
        GameEventHandler.Instance.GameEvents.OnPauseEvent -= Pause;
        GameEventHandler.Instance.GameEvents.OnRestartEvent -= Restart;
        GameEventHandler.Instance.GameEvents.OnFinishEvent -= Finish;
    }

    private void Play()
    {
        //Activate Start Game /use configuration for lvl
    }

    private void Pause()
    {
        // set time skale = 0 
        //or unpause
    }

    private void Restart()
    {
        //restart
    }

    private void Finish()
    {
        //stop game 
        //view score
        //end menu
    }





}
