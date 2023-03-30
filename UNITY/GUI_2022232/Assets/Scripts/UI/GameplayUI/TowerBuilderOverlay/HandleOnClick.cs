using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class HandleOnClick : MonoBehaviour, IPointerDownHandler
{
    public event Action OnClickSetCanvas;
    public event Action<TowerProperties> OnClickPlaceBuilding;
    
    public TowerProperties MyTower;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClickSetCanvas?.Invoke();
        OnClickPlaceBuilding?.Invoke(MyTower);
    }
}
