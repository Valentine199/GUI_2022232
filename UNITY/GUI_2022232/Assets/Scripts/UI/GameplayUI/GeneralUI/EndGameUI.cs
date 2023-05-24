using System.Collections;
using System.Collections.Generic;
using TowerDefense.Gameplay.Core;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] GameObject endGameCanvas;
    GameController gameController;
    private void Start()
    {
        gameController = GameController.Instance;
        gameController.OnGameOver += ToogleEndGameCanvas;
    }
    void ToogleEndGameCanvas()
    {

        endGameCanvas.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
    }
}
