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
            instance.GetComponent<HandleOnClick>().MyTower = obj;
            instance.transform.SetParent(_buildBg.transform);
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

    [SerializeField] private Canvas _selectionCanvas;
    [SerializeField] private GameObject _buildBg;
    [SerializeField] private GameObject[] _towers = new GameObject[0];
    [SerializeField] private GameObject _buildMenuItem;
    private bool _toggle = false;
}
