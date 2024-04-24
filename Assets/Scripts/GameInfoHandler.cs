using TMPro;
using UnityEngine;

public class GameInfoHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI _info;

    private string _score;
    private string _moves;

    private void OnEnable()
    {
        GameEventHandler.Instance.GameInfoEvent.OnScoreChange += CreateScore;
        GameEventHandler.Instance.GameInfoEvent.OnMovesChange += CreateMoves;
    }

    private void OnDisable()
    {
        GameEventHandler.Instance.GameInfoEvent.OnScoreChange -= CreateScore;
        GameEventHandler.Instance.GameInfoEvent.OnMovesChange -= CreateMoves;
    }

    public void CreateText()
    {
        string text = $"" +
            $"{_score}/n" +
            $"{_moves}";

        _info.text = text;
    }

    public void CreateScore(int scoreValue)
    {
        _score = $"Score:{scoreValue}";


    }

    public void CreateMoves(int moveValue)
    {
        _moves = $"Moves:{moveValue}";
    }


}
