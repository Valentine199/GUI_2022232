using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Towers;
using TowerDefense.Towers.TowerAttackControllers;
using TowerDefense.Towers.TowerUpgrades;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using TowerDefense.Towers.TowerEnums;

public class TowerInteractUI : MonoBehaviour
{
    private IInteractable _towerManager;
    public event Action OnUpgradeCanvasToggled;
    public IInteractable TowerManager
    {
        get
        {
            return _towerManager;
        }
        set
        {
            _towerManager = value;

        }
    }
    [SerializeField] private TMP_Text _towerName;
    [SerializeField] private TMP_Text _upgradeText;
    [SerializeField] private TMP_Text _upgradeCost;

    [SerializeField] private TMP_Text _SellPrice;

    [SerializeField] private TMP_Text _prevTargeting;
    [SerializeField] private TMP_Text _currentTargeting;
    [SerializeField] private TMP_Text _nextTargeting;


    public void InitSelf(IInteractable interactable)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        OnUpgradeCanvasToggled?.Invoke();

        _towerManager = interactable;

        _towerManager.OnNewUpgrade += ShowTowerInfo;
        _towerManager.OnTargetingStyleChange += ChangeTargetingText;

        ShowTowerInfo(_towerManager.GetUpgradeInfo());
        ChangeTargetingText();
        ShowRange();
    }

    private void ShowRange()
    {
        _towerManager.ShowTowerRange();
    }

    public void CloseSelf(bool isSold)
    {        
        if (!isSold) {_towerManager.HideTowerRange();}
        //_towerManager.HideTowerRange(); 
        Cursor.visible = false;        
        Cursor.lockState = CursorLockMode.Locked;
        OnUpgradeCanvasToggled?.Invoke();
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _towerManager.OnNewUpgrade -= ShowTowerInfo;
        _towerManager.OnTargetingStyleChange -= ChangeTargetingText;
        _towerManager = null;
    }

    private void ShowTowerInfo(TowerUpgrade obj)
    {
        if(obj != null)
        {
            _upgradeText.text = obj.UpgradeName;
            _upgradeCost.text = obj.Cost.ToString() + "$";
        }
        else
        {
            _upgradeText.text = "Fully upgraded";
            _upgradeCost.text = string.Empty;
        }

        _SellPrice.text = _towerManager.GetSellPrice().ToString() +"$";
        _towerName.text = _towerManager.ShowName();
    }

    private void ChangeTargetingText()
    {
        _currentTargeting.text = _towerManager.GetTargetingInfo().ToString();

        return;
        //int val = (int)_towerManager.GetTargetingInfo();
        //int prevVal = val;
        //val++;
        //if (val >= Enum.GetNames(typeof(TargetingStyle)).Length)
        //{
        //    val = 0;
        //}

        //_nextTargeting.text = ((TargetingStyle)val).ToString();

        //val = prevVal;
        //val--;
        //if (val < 0)
        //{
        //    val = Enum.GetNames(typeof(TargetingStyle)).Length - 1;
        //}
        //_prevTargeting.text = ((TargetingStyle)val).ToString();
    }

    public void NextTargeting()
    {
        _towerManager.CycleTargetingStyleForward();

        //int val = (int)_towerManager.GetTargetingInfo();
        //val++;
        //if (val >= Enum.GetNames(typeof(TargetingStyle)).Length)
        //{
        //    val = 0;
        //}
        
        //_currentTargeting.text = ((TargetingStyle)val).ToString();
        
    }

    public void PrevTargeting()
    {
        _towerManager.CycleTargetingStyleBackwards();

        //int val = (int)_towerManager.GetTargetingInfo();
        //val--;
        //if (val < 0)
        //{
        //    val = Enum.GetNames(typeof(TargetingStyle)).Length - 1;
        //}
        //_currentTargeting.text = ((TargetingStyle)val).ToString();
        //_currentTargeting.text = _towerManager.GetTargetingInfo().ToString();
    }

    public void BuyUpgrade()
    {
        _towerManager.InteractUpgradeServerRpc();
    }

    public void Sell()
    {        
        _towerManager.SellTower();
        CloseSelf(true);
    }
}
