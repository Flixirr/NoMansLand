using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTextures : MonoBehaviour
{
    public void ChangeMaterial(Material baseMat, Material additionalMat)
    {
        int childCount = 0;
        foreach(Transform child in transform.GetChild(0).transform)
        {
            if (childCount == 0)
            {
                child.GetComponent<MeshRenderer>().material = baseMat;
            } else {
                child.GetComponent<MeshRenderer>().material = additionalMat;
            }
            childCount++;
        }
    }
}
