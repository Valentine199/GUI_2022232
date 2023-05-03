using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerDefense.Gameplay.Core;
using UnityEngine;
using UnityEngine.UI;

public class GeneralUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _roundText;
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private TMP_Text _livesText;
    //[SerializeField] private Button _startRoundButton;
    private GameController _gameController;
    //[SerializeField] private WaveController _roundController;

    private void OnEnable()
    {
        
        _gameController = GameController.Instance;
        _gameController.OnWaveChanged += UpdateRoundText;
        _gameController.OnMoneyChanged += UpdateMoneyText;
        _gameController.OnLivesChanged += UpdateLivesText;

        //_gameController.OnGameBegin += ShowStartRoundButton;
        //_roundController.OnRoundComplete += ShowStartRoundButton;
        _gameController.InitUI();
    }

    private void OnDisable()
    {
        _gameController.OnWaveChanged -= UpdateRoundText;
        _gameController.OnMoneyChanged -= UpdateMoneyText;
        _gameController.OnLivesChanged -= UpdateLivesText;
        //_gameController.OnGameBegin -= ShowStartRoundButton;
        //_roundController.OnRoundComplete -= ShowStartRoundButton;
    }

    private void Start()
    {
        //HideStartRoundButton();
    }

    private void UpdateRoundText(int currentRound) =>
        _roundText.text = $"Round: {(currentRound.Equals(0) ? 1.ToString() : currentRound.ToString())}";
    private void UpdateMoneyText(int currentMoney) => _moneyText.text = $"Money: {currentMoney.ToString()}";
    private void UpdateLivesText(int currentLives) => _livesText.text = $"Lives: {currentLives.ToString()}";
    //private void ShowStartRoundButton(RoundProperties round) => ShowStartRoundButton();
    //private void ShowStartRoundButton() => _startRoundButton.gameObject.SetActive(true);
    //public void HideStartRoundButton() => _startRoundButton.gameObject.SetActive(false);

}
