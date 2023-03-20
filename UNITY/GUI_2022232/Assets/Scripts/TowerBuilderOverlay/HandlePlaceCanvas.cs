using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePlaceCanvas : MonoBehaviour
{
    private void Awake()
    {
        foreach (GameObject obj in _towers)
        {
            GameObject instance =  Instantiate(_buildMenuItem);
            HandleOnClick InstanceHandleClick = instance.GetComponent<HandleOnClick>();

            if (InstanceHandleClick != null)
            {
                InstanceHandleClick.MyTower = obj;
                InstanceHandleClick.OnClickSetCanvas += ToggleBuildingsCanvas;
                InstanceHandleClick.OnClickPlaceBuilding += gameObject.GetComponent<PlaceTower>().PlaceBuilding;
            }

            instance.transform.SetParent(_buildBg.transform, false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
           ToggleBuildingsCanvas();
        }
    }

    public void ToggleBuildingsCanvas()
    {
        _toggle = !_toggle;
        _selectionCanvas.gameObject.SetActive(_toggle);
    }


    [SerializeField] private Canvas _selectionCanvas; // The canvas which handles the UI
    [SerializeField] private GameObject _buildBg;     // The direct parent to the build elements  
    [SerializeField] private GameObject[] _towers = new GameObject[0];  // A list of all possible buildings
    [SerializeField] private GameObject _buildMenuItem;  // What to instantiate as a building option


    private bool _toggle = false;
}
