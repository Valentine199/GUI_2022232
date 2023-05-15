using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetUpMenuItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _ItemName;
    private void Start()
    {
        HandleOnClick script =  gameObject.GetComponent<HandleOnClick>();

        _ItemName.text = script.MyTower.TowerName;

        Image image = GetComponent<Image>();

        image.sprite = script.MyTower.TowerPicture;
    }
}
