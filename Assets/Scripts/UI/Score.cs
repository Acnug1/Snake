using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private SnakeContoller _player;
    [SerializeField] private TMP_Text _scoreText;

    private int _score = 0;

    private void OnEnable()
    {
        _player.ScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        _player.ScoreChanged -= OnScoreChanged;
    }

    private void Start()
    {
        _scoreText.text = _score.ToString();
    }

    private void OnScoreChanged()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }
}
