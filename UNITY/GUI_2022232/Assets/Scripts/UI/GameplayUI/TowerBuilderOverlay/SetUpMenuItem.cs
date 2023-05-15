using UnityEngine;
using TMPro;

public class SetUpMenuItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _ItemName;
    //Something, something picture change
    private void Start()
    {
        HandleOnClick script =  gameObject.GetComponent<HandleOnClick>();

        _ItemName.text = script.MyTower.name;
    }
}
