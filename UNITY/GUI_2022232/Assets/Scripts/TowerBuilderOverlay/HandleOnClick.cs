using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandleOnClick : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject _gameObject;
    private HandlePlaceCanvas _canvasToggle;
    private PlaceTower _builder;
    public GameObject MyTower;

    private void Start()
    {
        _canvasToggle = _gameObject.GetComponent<HandlePlaceCanvas>();
        _builder = _gameObject.GetComponent<PlaceTower>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _canvasToggle.ToggleBuildingsCanvas();
        _builder.PlaceBuilding(MyTower);
    }
}
