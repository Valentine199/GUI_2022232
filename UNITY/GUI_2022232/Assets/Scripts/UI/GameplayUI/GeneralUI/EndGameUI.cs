using System.Collections;
using System.Collections.Generic;
using TowerDefense.Gameplay.Core;
using Unity.Netcode;
using UnityEngine;

public class EndGameUI : NetworkBehaviour
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
        if(!IsOwner) { return; }

        endGameCanvas.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
    }
}
