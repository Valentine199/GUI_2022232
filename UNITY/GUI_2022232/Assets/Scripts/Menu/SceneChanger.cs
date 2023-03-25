using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_TextMeshProUGUI;
    
    public void OpenScene()
    {
        //Debug.Log(m_TextMeshProUGUI.text);
        SceneManager.LoadScene(m_TextMeshProUGUI.text);
    }
}
