using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTester : MonoBehaviour
{
    [SerializeField] GameObject gameObjectToLookAt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(gameObjectToLookAt.transform);
    }
}
