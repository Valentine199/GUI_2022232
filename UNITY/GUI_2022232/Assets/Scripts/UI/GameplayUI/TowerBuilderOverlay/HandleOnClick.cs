using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TowerDefense.Data.Towers;

public class HandleOnClick : MonoBehaviour//, IPointerDownHandler
{
    public event Action OnClickSetCanvas;
    public event Action<TowerProperties> OnClickPlaceBuilding;
    
    public TowerProperties MyTower;

    ////public void OnPointerDown(PointerEventData eventData)
    //{
    //    OnClickSetCanvas?.Invoke();
    //    OnClickPlaceBuilding?.Invoke(MyTower);
    //}

    public void ElementClicked()
    {
        OnClickSetCanvas?.Invoke();
        OnClickPlaceBuilding?.Invoke(MyTower);
    }
}
