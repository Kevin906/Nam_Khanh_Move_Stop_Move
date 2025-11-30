using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasVictory : UICanvas
{
    [SerializeField] private GameObject _congratulation;
    [SerializeField] private GameObject _Button;

    private void OnEnable()
    {
        _congratulation.SetActive(false);
    }

    public void HomeButton()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void RestartButton()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void NextButton()
    {
        GameManager.Instance.LoadNewLevel();
        UIManager.Instance.OpenUI(UIName.GamePlay);
        GameManager.Instance.gameState = GameManager.GameState.gameStarted;
    }
}
