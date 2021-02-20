using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private SnakeContoller _player;

    private CanvasGroup _canvasGroup;

    private void OnEnable()
    {
        _player.Died += OnPlayerDied;
    }

    private void OnDisable()
    {
        _player.Died -= OnPlayerDied;
    }

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    private void OnPlayerDied()
    {
        StartCoroutine(ShowGameOverScreen());
    }

    private IEnumerator ShowGameOverScreen()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        for (float i = 0; i <= 1; i = i + 0.1f)
        {
            _canvasGroup.alpha = i;
            yield return waitForEndOfFrame;
        }
    }
}
