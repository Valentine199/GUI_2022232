using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderRootSyncInator : MonoBehaviour
{
    [SerializeField] Transform outSideRoot;
    [SerializeField] Transform insideSideRoot;

    // Update is called once per frame
    void Update()
    {
        outSideRoot.position = insideSideRoot.position;
        outSideRoot.rotation = insideSideRoot.rotation;
    }
}
