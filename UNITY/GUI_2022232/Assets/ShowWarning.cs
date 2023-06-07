using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowWarning : MonoBehaviour
{
    [SerializeField] private TMP_Text _warningText;
    [SerializeField] private PlaceTower _watchedPlaceTower;

    private WaitForSeconds waitTime = new WaitForSeconds(2);
    [SerializeField]private GameObject canvasObject;


    private void Awake()
    {
        _watchedPlaceTower.OnCantplaceBuilding += ShowText;
        canvasObject = _warningText.transform.parent.gameObject;
    }

    private void ShowText(string text)
    {
        _warningText.text = text;
        StartCoroutine(TextTimer());
    }

    private IEnumerator TextTimer()
    {
        canvasObject.SetActive(true);
        yield return waitTime;
        canvasObject.SetActive(false);  
    }
}
